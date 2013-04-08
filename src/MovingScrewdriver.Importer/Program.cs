using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using BlogML.Xml;
using MovingScrewdriver.Web.Infrastructure;
using MovingScrewdriver.Web.Models;
using Raven.Client;
using Raven.Client.Document;

namespace MovingScrewdriver.Importer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting importer...");
            try
            {
                using (var importer = new Importer())
                {
                    Console.WriteLine("Importer created, starting import process...");
                    importer.CreateDatabase();
                    importer.ImportBlogMl();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
            }

            Console.ReadLine();
        }
    }

    public class Importer : IDisposable
    {
        private readonly BlogMLBlog _blog;
        private IDocumentStore _store;

        public Importer() : this("BlogML.xml", "RavenDB")
        {
            
        }

        public Importer(string filepath, string connectionString)
        {
            using (var stream = File.Open(filepath, FileMode.Open))
            {
                _blog = BlogMLSerializer.Deserialize(stream);
            }
            
            _store = new DocumentStore
            {
                ConnectionStringName = connectionString
            }.Initialize();
        }

        public void Dispose()
        {
            if (_store != null)
            {
                _store.Dispose();
                _store = null;
            }
        }

        public void CreateDatabase()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\tCreating Configuration...");
            using (IDocumentSession s = _store.OpenSession())
            {
                var config = ScrewdriverConfig.Create();
                config.Id = "Blog/Config";
                config.Title = "";
                config.SubTitle = "";
                config.Tagline = "";
                config.OwnerEmail = "";
                config.OwnerFirstName = "";
                config.OwnerLastName = "";
                config.OwnerNick = "";
                config.GoogleAnalyticsKey = "";
                config.GoogleSiteVerificationKey = "";
                config.AkismetKey = "";

                config.FooterHtml = "<p>Copyright © {0} NAME. All rights reserved. </p>";
                config.FooterHtml += "<p><small>Informacje tutaj zawarte są prywatnymi opiniami, w żaden sposób nie związanymi z aktualnym bądź poprzednimi pracodawcami.</small></p>";

                config.MetaTitle = "";
                config.MetaDescription = "";
                config.MetaCopyright = "";
                config.MetaKeywords = "";
                config.MetaApplicationName = "";
                config.MetaLang = "pl";

                config.ShowSocialMeta = true;
                config.MetaTwitterName = "";
                config.MetaFbAppLikeBoxUrl = "https://www.facebook.com/URL";

                config.ShowOgMeta = true;
                config.MetaOgLocale = "pl_PL";
                config.MetaOgImage = "http://URL/apple-touch-icon-114x114-precomposed.png";

                config.ShowDcMeta = true;

                config.ShowWinIe9Meta = true;
                config.MetaIe9Tooltip = "";
                config.MetaIe9StartUrl = "./";
                config.MetaIe9NavBarColor = "#d6d6d6";
                config.MetaIe9Tasks.Add("name=Archiwum;action-uri=/archiwum;icon-uri=/content/images/archive.ico");
                config.MetaIe9Tasks.Add("name=KONTO na Twitterze;action-uri=http://twitter.com/KONTO;icon-uri=/content/images/twitter.ico");

                config.ShowWin8Meta = true;
                config.MetaWin8TileColor = "#d6d6d6";
                config.MetaWin8TileImage = "http://URL/apple-touch-icon-144x144-precomposed.png";
                config.MetaWin8BadgeShouldShow = false;

                config.ShowAppleMeta = true;
                config.MetaAppleMobileWebAppTitle = "NAME";
                config.MetaAppleMobileWebAppCapable = "yes";
                config.MetaAppleMobileWebAppStatusBarStyle = "black";

                config.ShowMeMeta = true;
                config.MetaMeLinks.Add("http://twitter.com/KONTO");

                config.NumberOfDayToCloseComments = 30;
                config.MinNumberOfPostForSignificantTag = 15;

                config.FeedUrl = "http://feeds.feedburner.com/URL";
                config.FeedCommentsUrl = "http://feeds.feedburner.com/KONTO";
                
                s.Store(config);
                s.SaveChanges();
            }
        }

        public void ImportBlogMl()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\tCreating Users...");
            using (var session = _store.OpenSession())
            {
                var user = new BlogOwner
                {
                    Id = "Blog/Owner",
                    Email = "",
                    FirstName = "",
                    LastName = "",
                    Nick = "",
                    Twitter = ""
                };

                user.ResetPassword("admin123");

                session.Store(user);
                session.SaveChanges();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\tImporting Posts...");

            foreach (var oldPost in _blog.Posts)
            {
                var post = new Post
                {
                    AuthorId = "Blog/Owner",
                    Created = new DateTimeOffset(oldPost.DateCreated),
                    Modified = new DateTimeOffset(oldPost.DateCreated),
                    PublishAt = new DateTimeOffset(oldPost.DateCreated),
                    Content = oldPost.Content.Text.Replace("http:/URL/image.axd?picture=", "http://URL/content/old-images/"),
                    
                    Title = HttpUtility.HtmlDecode(oldPost.Title),
                    Slug = SlugConverter.TitleToSlug(HttpUtility.HtmlDecode(oldPost.Title)),
                    LegacyUniqueId = oldPost.ID,
                    LegacySlug = Regex.Match(oldPost.PostUrl, @"([^/]+)(?=\.\w+$)").Value,

                    AllowComments = true
                };

                var categoriesRefs = oldPost.Categories.OfType<BlogMLCategoryReference>();
                var oldCategories = from x in categoriesRefs
                                    join z in _blog.Categories on x.Ref equals z.ID
                                    select new Post.SlugItem
                                    {
                                        Title = z.Title
                                    };

                var oldTags = from x in oldPost.Tags.OfType<BlogMLTagReference>()
                              select new Post.SlugItem
                              {
                                Title = x.Ref
                              };

                post.Categories = oldCategories.ToList();
                post.Tags = oldTags.ToList();

                var commentsCollection = new PostComments();

                foreach (var oldComment in oldPost.Comments.OfType<BlogMLComment>())
                {
                    var comment = new PostComments.Comment
                    {
                        Id = commentsCollection.GenerateNewCommentId(),
                        Author = oldComment.UserName,
                        Content = convert_to_markdown(oldComment.Content.Text),
                        Created = oldComment.DateCreated,
                        Email = oldComment.UserEMail,
                        Type = CommentType.Comment,
                        Url = oldComment.UserUrl,
                        Important = oldComment.UserEMail.Equals("EMAIL", StringComparison.OrdinalIgnoreCase),
                        UserAgent = string.Empty,
                        UserHostAddress = oldComment.UserIp,
                        IsSpam = oldComment.Approved == false,
                    };

                    if (oldComment.Approved)
                    {
                        commentsCollection.Comments.Add(comment);
                    }
                    else
                    {
                        commentsCollection.Spam.Add(comment);
                    }
                    
                }

                foreach (var trackbacks in oldPost.Trackbacks.OfType<BlogMLTrackback>())
                {
                    var comment = new PostComments.Comment
                    {
                        Id = commentsCollection.GenerateNewCommentId(),
                        Type = CommentType.Trackback,
                        Author = trackbacks.UserName,
                        Content = trackbacks.Content.Text,
                        Created = trackbacks.DateCreated,
                        Email = CommentType.Trackback.ToString(),
                        Url = trackbacks.Url,
                        Important = false,
                        UserAgent = string.Empty,
                        UserHostAddress = trackbacks.UserIp,
                        IsSpam = trackbacks.Approved == false,
                    };

                    if (trackbacks.Approved)
                    {
                        commentsCollection.Comments.Add(comment);
                    }
                    else
                    {
                        commentsCollection.Spam.Add(comment);
                    }
                }

                post.CommentsCount = commentsCollection.Comments.Count;

                using (IDocumentSession session = _store.OpenSession())
                {
                    session.Store(commentsCollection);
                    post.CommentsId = commentsCollection.Id;

                    session.Store(post);
                    commentsCollection.Post = new PostComments.PostReference
                    {
                        Id = post.Id,
                        Published = post.PublishAt,
                        Slug = post.Slug
                    };

                    session.SaveChanges();
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\tDone...");
        }

        private static string convert_to_markdown(string content)
        {
            content = Regex.Replace(content, @"\[b\]((?:.|\n)+?)\[\/b\]", "**$1**", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            content = Regex.Replace(content, @"\[u\]((?:.|\n)+?)\[\/u\]", "*$1*", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            content = Regex.Replace(content, @"\[i\]((?:.|\n)+?)\[\/i\]", "_$1_", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            content = Regex.Replace(content, @"\[s\]((?:.|\n)+?)\[\/s\]", "~~$1~~", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            content = Regex.Replace(content, @"\[color\=.+?\]((?:.|\n)+?)\[\/color\]", "$1", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            content = Regex.Replace(content, @"(\n)\[\*\]", "$1*", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            content = Regex.Replace(content, @"\[\/*list\]", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            content = Regex.Replace(content, @"\[img\]((?:.|\n)+?)\[\/img\]", "![]($1)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            content = Regex.Replace(content, @"\[url=(.+?)\]((?:.|\n)+?)\[\/url\]", "[$2]($1)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return content;
        }
    }
}
