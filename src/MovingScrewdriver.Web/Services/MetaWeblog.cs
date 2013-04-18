using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using CookComputing.XmlRpc;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Infrastructure;
using MovingScrewdriver.Web.Infrastructure.Indexes;
using MovingScrewdriver.Web.Models;
using MovingScrewdriver.Web.Services.Model;
using NLog;
using Raven.Client;
using Post = MovingScrewdriver.Web.Services.Model.Post;

namespace MovingScrewdriver.Web.Services
{
    /// <summary>
    /// WP: http://codex.wordpress.org/XML-RPC_wp
    /// metaWeblog: http://codex.wordpress.org/XML-RPC_MetaWeblog_API
    /// blogger: http://codex.wordpress.org/XML-RPC_Blogger_API
    /// </summary>
    public interface IMetaWeblog
    {
        [XmlRpcMethod("metaWeblog.newPost")]
        string AddPost(string blogid, string username, string password, Post post, bool publish);

        [XmlRpcMethod("blogger.deletePost")]
        [return: XmlRpcReturnValue(Description = "Returns true.")]
        bool DeletePost(string key, string postid, string username, string password, bool publish);

        [XmlRpcMethod("metaWeblog.getCategories")]
        CategoryInfo[] GetCategories(string blogid, string username, string password);

        [XmlRpcMethod("metaWeblog.getPost")]
        Post GetPost(string postid, string username, string password);

        [XmlRpcMethod("metaWeblog.getRecentPosts", Description = "Retrieves a list of the most recent existing post using the metaWeblog API. Returns the metaWeblog struct collection.")]
        Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts);

        [XmlRpcMethod("blogger.getUserInfo")]
        UserInfo GetUserInfo(string key, string username, string password);

        [XmlRpcMethod("blogger.getUsersBlogs")]
        BlogInfo[] GetUsersBlogs(string key, string username, string password);

        [XmlRpcMethod("metaWeblog.newMediaObject", Description = "Uploads an image, movie, song, or other media using the metaWeblog API. Returns the metaObject struct.")]
        MediaObjectInfo NewMediaObject(string blogid, string username, string password, MediaObject mediaObject);

        [XmlRpcMethod("metaWeblog.editPost")]
        bool UpdatePost(string postid, string username, string password, Post post, bool publish);

        [XmlRpcMethod("wp.getAuthors")]
        WpAuthor[] WpGetAuthors(string blog_id, string username, string password);

        [XmlRpcMethod("wp.getCategories")]
        WpCategoryInfo[] WpGetCategories(string blog_id, string username, string password);

        [XmlRpcMethod("wp.getTags")]
        WpTagInfo[] WpGetTags(string blog_id, string username, string password);

        [XmlRpcMethod("wp.newCategory",
            Description = "Adds a new category to the blog engine.")]
        int WpNewCategory(
            string blog_id,
            string username,
            string password,
            WpNewCategory category);

    }

    public class MetaWeblog : XmlRpcService, IMetaWeblog
    {
        private readonly IDocumentStore _store;
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly INotificationService _notification;
        private readonly IScheduleStrategy _schedule;

        public MetaWeblog()
        {
            _store = AutofacConfig.IoC.Resolve<IDocumentStore>();
            _notification = AutofacConfig.IoC.Resolve<INotificationService>();
            _schedule = AutofacConfig.IoC.Resolve<IScheduleStrategy>();
        }

        private UrlHelper Url
        {
            get { return new UrlHelper(new RequestContext(new HttpContextWrapper(Context), new RouteData())); }
        }

        string IMetaWeblog.AddPost(string blogid, string username, string password, Post post, bool publish)
        {
            _log.Trace("Adding new blog post. Blogid: {0}, Publish: {1}, Post: {2}", blogid, publish, post.ToJson());

            Models.Post newPost;
            using (var session = _store.OpenSession())
            {
                var user = validate_user(username, password);
                var comments = new PostComments
                {
                    Comments = new List<PostComments.Comment>(),
                    Spam = new List<PostComments.Comment>()
                };
                session.Store(comments);

                var currentDate = ApplicationTime.Current;
                var publishDate = post.dateCreated  == null ? currentDate : _schedule.Schedule(new DateTimeOffset(post.dateCreated.Value));

                var cat = post.categories == null ? new List<string>() : post.categories.ToList();
                var key = post.mt_keywords.IsNullOrWhiteSpace() ? new List<string>() : post.mt_keywords.Split(',').ToList();

                newPost = new Models.Post
                {
                    AuthorId = user.Id,
                    Content = post.description,
                    CommentsId = comments.Id,
                    Created = currentDate,
                    Modified = currentDate,
                    PublishAt = publishDate,
                    Categories = cat.Select(x => new Models.Post.SlugItem { Title = x }).ToList(),
                    Tags = key.Select(x => new Models.Post.SlugItem { Title = x }).ToList(),
                    Title = HttpUtility.HtmlDecode(post.title),
                    CommentsCount = 0,
                    AllowComments = true,
                };
                
                // only send notification when post is published!
                if (publishDate == currentDate)
                {
                    try
                    {
                        _notification.Send(newPost
                            , new Uri(Url.AbsoluteAction("Details", "PostDetails", newPost.ToRouteData())));
                        newPost.NotificationSend = true;
                    }
                    catch (Exception ex)
                    {
                        newPost.NotificationSend = false;
                    }
                }

                session.Store(newPost);
                comments.Post = new PostComments.PostReference
                {
                    Id = newPost.Id,
                    Published = publishDate,
                    Slug = newPost.Slug
                };

                session.SaveChanges();
            }

            return newPost.Id;
        }

        bool IMetaWeblog.UpdatePost(string postid, string username, string password, Post post, bool publish)
        {
            _log.Trace("Updating post. postid: {0}, Publish: {1}, Post: {2}", postid, publish, post.ToJson());

            using (var session = _store.OpenSession())
            {
                var user = validate_user(username, password);
                var postToEdit = session
                    .Include<Models.Post>(x => x.CommentsId)
                    .Load(postid);

                if (postToEdit == null)
                {
                    throw new XmlRpcFaultException(0, "Post does not exists");
                }
                    

                if (postToEdit.AuthorId.IsNullOrWhiteSpace())
                {
                    postToEdit.AuthorId = user.Id;
                }
                
                
                    //postToEdit.AuthorId = user.Id;
                var currentDate = ApplicationTime.Current;
                postToEdit.Modified = currentDate;

                postToEdit.Content = post.description;

                if (post.dateCreated.HasValue
                    && post.dateCreated.Value != postToEdit.PublishAt.DateTime)
                {
                    postToEdit.PublishAt = _schedule.Schedule(new DateTimeOffset(post.dateCreated.Value));
                    session.Load<PostComments>(postToEdit.CommentsId).Post.Published = postToEdit.PublishAt;
                }
                
                var cat = post.categories == null ? new List<string>() : post.categories.ToList();
                var key = post.mt_keywords.IsNullOrWhiteSpace() ? new List<string>() : post.mt_keywords.Split(',').ToList();

                postToEdit.Categories = cat.Select(x => new Models.Post.SlugItem
                {
                    Title = x
                }).ToList();

                postToEdit.Tags = key.Select(x => new Models.Post.SlugItem
                {
                    Title = x
                }).ToList();

                postToEdit.Title = HttpUtility.HtmlDecode(post.title);
                postToEdit.Slug = SlugConverter.TitleToSlug(postToEdit.Title);

                // only send notification when post is published!
                if (currentDate < postToEdit.PublishAt)
                {
                    try
                    {
                        _notification.Send(postToEdit
                            , new Uri(Url.AbsoluteAction("Details", "PostDetails", postToEdit.ToRouteData())));
                        postToEdit.NotificationSend = true;
                    }
                    catch (Exception ex)
                    {
                        postToEdit.NotificationSend = false;
                    }
                }

                session.SaveChanges();
            }

            return true;
        }

        public WpAuthor[] WpGetAuthors(string blog_id, string username, string password)
        {
            var user = validate_user(username, password);

            return new[]
            {
                new WpAuthor
                {
                    display_name = user.Nick,
                    user_email = user.Email,
                    user_login = user.Email,
                    user_id = user.Id
                }
            };
        }

        int IMetaWeblog.WpNewCategory(string blog_id, string username, string password, WpNewCategory category)
        {
            validate_user(username, password);
            return 1;
        }


        WpCategoryInfo[] IMetaWeblog.WpGetCategories(string blog_id, string username, string password)
        {
            var categories = get_categories(username, password);


            var toReturn = categories.Select(x => new WpCategoryInfo
                {
                    categoryId = x.Name,
                    description = x.Name,
                    categoryName = x.Name,
                    htmlUrl = Url.Action("ByCategory", "PostsByCategory", new { slug = x.Slug }),
                    rssUrl = Url.Action("RssByCategory", "Syndication", new { slug = x.Slug }),
                }).ToArray();

            _log.Trace("Reurning categories for blog_id: {0}. Categories: {1}", blog_id, toReturn.ToJson());

            return toReturn;
        }

        CategoryInfo[] IMetaWeblog.GetCategories(string blogid, string username, string password)
        {
            var categories = get_categories(username, password);

            var toReturn = categories.Select(x => new CategoryInfo
            {
                categoryid = x.Name,
                description = x.Name,
                title = x.Name,
                htmlUrl = Url.Action("ByCategory", "PostsByCategory", new { slug = x.Slug }),
                rssUrl = Url.Action("RssByCategory", "Syndication", new { slug = x.Slug }),
            }).ToArray();

            _log.Trace("Reurning categories for blogid: {0}. Categories: {1}", blogid, toReturn.ToJson());
            return toReturn;
        }

        WpTagInfo[] IMetaWeblog.WpGetTags(string blog_id, string username, string password)
        {
            var tags = get_tags(username, password);

            var toReturn = tags.Select(x => new WpTagInfo
            {
                tag_id = x.Slug,
                slug = x.Slug,
                name = x.Name,
                count = x.Count,
                html_url = Url.Action("ByTags", "PostsByTags", new { slug = x.Slug }),
                rss_url = Url.Action("RssByTag", "Syndication", new { slug = x.Slug }),
            }).ToArray();

            _log.Trace("Reurning tags for blog_id: {0}. Categories: {1}", blog_id, toReturn.ToJson());

            return toReturn;
        }

        Post IMetaWeblog.GetPost(string postid, string username, string password)
        {
            validate_user(username, password);
            
            using (var session = _store.OpenSession())
            {
                var thePost = session.Load<Models.Post>(postid);
                if (thePost == null || thePost.IsDeleted)
                {
                    _log.Trace("Post id: {0} does not exists, or has been deleted", postid);
                    throw new XmlRpcFaultException(0, "Post does not exists");
                }

                var toReturn = new Post
                {
                    wp_slug = SlugConverter.TitleToSlug(thePost.Title),
                    description = thePost.Content,
                    dateCreated = thePost.PublishAt.DateTime,
                    categories = thePost.Categories.Select(x => x.Title).ToArray(),
                    mt_keywords = string.Join(", ", thePost.Tags.Select(x => x.Title).ToArray()),
                    title = thePost.Title,
                    postid = thePost.Id,
                };

                _log.Trace("Returning psotid: {0}, Post: {1}", postid, toReturn.ToJson());

                return toReturn;
            }
        }

        Post[] IMetaWeblog.GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            validate_user(username, password);

            using (var session = _store.OpenSession())
            {
                var list = session.Query<Models.Post>()
                    .Where(p => p.IsDeleted == false)
                    .OrderByDescending(x => x.PublishAt)
                    .Take(numberOfPosts)
                    .ToList();

                var toReturn = list.Select(thePost => new Post
                {
                    wp_slug = SlugConverter.TitleToSlug(thePost.Title),
                    description = thePost.Content,
                    dateCreated = thePost.PublishAt.DateTime,
                    categories = thePost.Categories.Select(x => x.Title).ToArray(),
                    mt_keywords = string.Join(", ", thePost.Tags.Select(x => x.Title).ToArray()),
                    title = thePost.Title,
                    postid = thePost.Id,
                }).ToArray();


                _log.Trace("Returning blogid: {0}, posts count: {1}. Posts {2}", blogid, numberOfPosts, toReturn.ToJson());

                return toReturn;
            }
        }


        MediaObjectInfo IMetaWeblog.NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
        {
            validate_user(username, password);
            var path = ConfigurationManager.AppSettings["UploadsPath"];
            var imagePhysicalPath = Context.Server.MapPath(path);
            var imageWebPath = VirtualPathUtility.ToAbsolute(path);

            imagePhysicalPath = Path.Combine(imagePhysicalPath, mediaObject.name);
            var directoryPath = Path.GetDirectoryName(imagePhysicalPath).Replace("/", "\\");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllBytes(imagePhysicalPath, mediaObject.bits);

            var media = new MediaObjectInfo
            {
                url = Path.Combine(imageWebPath, mediaObject.name)
            };

            _log.Trace("NEw media object created for blogid: {0}, media object: {1}", blogid, media.ToJson());

            return media;
        }


        bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
        {
            validate_user(username, password);

            using (var session = _store.OpenSession())
            {
                var thePost = session.Load<Models.Post>(postid);

                if (thePost != null)
                {
                    thePost.IsDeleted = true;
                }

                _log.Trace("Post {0} with key: {1}, and publish: {2} marked as deleted.", postid, key, publish);
                session.SaveChanges();
            }

            return true;
        }

        BlogInfo[] IMetaWeblog.GetUsersBlogs(string key, string username, string password)
        {
            validate_user(username, password);
            return new[]
            {
                new BlogInfo
                {
                    blogid = "blogs/1",
                    blogName = username,
                    url = Context.Request.RawUrl
                }
            };
        }

        UserInfo IMetaWeblog.GetUserInfo(string key, string username, string password)
        {
            var user = validate_user(username, password);
            var info = new UserInfo
            {
                email = user.Email,
                nickname = user.Nick,
                firstname = user.FirstName,
                lastname = user.LastName,
                userid = user.Id
            };

            _log.Trace("Returning user info for key: {0}. User Info: {1}", key, info.ToJson());

            return info;
        }

        private BlogOwner validate_user(string username, string password)
        {
            BlogOwner user;
            using (var session = _store.OpenSession())
            {
                user = session.GetUserByEmail(username);
            }

            if (user == null || user.ValidatePassword(password) == false)
            {
                _log.Warn("Unathorized access, user name: {0}, password: {1}", username, password);
                throw new XmlRpcFaultException(0, "User is not valid!");
            }

            return user;
        }

        private IEnumerable<Category_Count.ReduceResult> get_categories(string username, string password)
        {
            validate_user(username, password);
            var mostRecentTag = new DateTimeOffset(DateTimeOffset.Now.Year - 2,
                                                   DateTimeOffset.Now.Month,
                                                   1, 0, 0, 0,
                                                   DateTimeOffset.Now.Offset);

            using (var session = _store.OpenSession())
            {
                var categoryInfos = session.Query<Category_Count.ReduceResult, Category_Count>()
                    .Where(x => x.LastSeenAt > mostRecentTag)
                    .ToList();

                return categoryInfos;
            }
        }

        private IEnumerable<Tags_Count.ReduceResult> get_tags(string username, string password)
        {
            validate_user(username, password);
            var mostRecentTag = new DateTimeOffset(DateTimeOffset.Now.Year - 2,
                                                   DateTimeOffset.Now.Month,
                                                   1, 0, 0, 0,
                                                   DateTimeOffset.Now.Offset);

            using (var session = _store.OpenSession())
            {
                var tags = session.Query<Tags_Count.ReduceResult, Tags_Count>()
                    .Where(x => x.LastSeenAt > mostRecentTag)
                    .ToList();

                return tags;
            }
        }
    }
}