using System;
using System.Xml.Serialization;
using System.Collections;

namespace BlogML.Xml
{
    [Serializable]
    public sealed class BlogMLPost : BlogMLNode
    {
        private string postUrl ;
        private bool hasexcerpt = false;
        private BlogMLContent content = new BlogMLContent();
        private BlogMLContent excerpt = new BlogMLContent();
        private BlogPostTypes blogpostType = new BlogPostTypes();
        private AuthorReferenceCollection authors;
        private CommentCollection comments;
        private TrackbackCollection trackbacks;
        private CategoryReferenceCollection categories;
        private TagReferenceCollection tags;
        private AttachmentCollection attachments;
        private UInt32 postviews = 0;
        private string postname;

        [XmlAttribute("post-url")]
        public string PostUrl {
            get { return this.postUrl; }
            set { this.postUrl = value; }
        }

        [XmlAttribute("hasexcerpt")]
        public bool HasExcerpt
        {
            get { return this.hasexcerpt; }
            set { this.hasexcerpt = value; }
        }

        [XmlAttribute("type")]
        public BlogPostTypes PostType
        {
            get { return this.blogpostType; }
            set { this.blogpostType = value; }
        }

        [XmlAttribute("views")]
        public UInt32 Views
        {
            get { return this.postviews; }
            set { this.postviews = value; }
        }

        [XmlElement("post-name")]
        public string PostName
        {
            get { return this.postname; }
            set { this.postname = value; }
        }

        [XmlElement("content")]
        public BlogMLContent Content
        {
            get { return this.content; }
            set { this.content = value; }
        }

        [XmlElement("excerpt")]
        public BlogMLContent Excerpt
        {
            get { return this.excerpt; }
            set { this.excerpt = value; }
        }

        [XmlArray("authors")]
        [XmlArrayItem("author", typeof(BlogMLAuthorReference))]
        public AuthorReferenceCollection Authors
        {
            get
            {
                if (this.authors == null)
                    this.authors = new AuthorReferenceCollection();
                return this.authors;
            }
        }

        [XmlArray("categories")]
        [XmlArrayItem("category", typeof(BlogMLCategoryReference))]
        public CategoryReferenceCollection Categories
        {
            get
            {
                if (this.categories == null)
                    this.categories = new CategoryReferenceCollection();
                return this.categories;
            }
        }

        [XmlArray("tags")]
        [XmlArrayItem("tag", typeof(BlogMLTagReference))]
        public TagReferenceCollection Tags
        {
            get
            {
                if (this.tags == null)
                    this.tags = new TagReferenceCollection();
                return this.tags;
            }
        }

        [XmlArray("comments")]
        [XmlArrayItem("comment", typeof(BlogMLComment))]
        public CommentCollection Comments
        {
            get
            {
                if (this.comments == null)
                    this.comments = new CommentCollection();
                return this.comments;
            }
        }


        [XmlArray("trackbacks")]
        [XmlArrayItem("trackback", typeof(BlogMLTrackback))]
        public TrackbackCollection Trackbacks
        {
            get
            {
                if (this.trackbacks == null)
                    this.trackbacks = new TrackbackCollection();
                return this.trackbacks;
            }
        }

        [XmlArray("attachments")]
        [XmlArrayItem("attachment", typeof(BlogMLAttachment))]
        public AttachmentCollection Attachments
        {
            get
            {
                if (this.attachments == null)
                    this.attachments = new AttachmentCollection();
                return this.attachments;
            }
        }

        [Serializable]
        public sealed class AuthorReferenceCollection : ArrayList
        {
            public new BlogMLAuthorReference this[int index]
            {
                get { return base[index] as BlogMLAuthorReference; }
            }

            public void Add(BlogMLAuthorReference value)
            {
                base.Add(value);
            }

            public BlogMLAuthorReference Add(string authorID)
            {
                BlogMLAuthorReference item = new BlogMLAuthorReference();
                item.Ref = authorID;
                base.Add(item);
                return item;
            }
        }

        [Serializable]
        public sealed class CommentCollection : ArrayList
        {
            public new BlogMLComment this[int index]
            {
                get { return base[index] as BlogMLComment; }
            }

            public void Add(BlogMLComment value)
            {
                base.Add(value);
            }
        }

        [Serializable]
        public sealed class TrackbackCollection : ArrayList
        {
            public new BlogMLTrackback this[int index]
            {
                get { return base[index] as BlogMLTrackback; }
            }

            public void Add(BlogMLTrackback value)
            {
                base.Add(value);
            }
        }

        [Serializable]
        public sealed class CategoryReferenceCollection : ArrayList
        {
            public new BlogMLCategoryReference this[int index]
            {
                get { return base[index] as BlogMLCategoryReference; }
            }

            public void Add(BlogMLCategoryReference value)
            {
                base.Add(value);
            }

            public BlogMLCategoryReference Add(string categoryID)
            {
                BlogMLCategoryReference item = new BlogMLCategoryReference();
                item.Ref = categoryID;
                base.Add(item);
                return item;
            }
        }
        [Serializable]
        public sealed class TagReferenceCollection : ArrayList
        {
            public new BlogMLTagReference this[int index]
            {
                get { return base[index] as BlogMLTagReference; }
            }

            public void Add(BlogMLTagReference value)
            {
                base.Add(value);
            }

            public BlogMLTagReference Add(string tagID)
            {
                BlogMLTagReference item = new BlogMLTagReference();
                item.Ref = tagID;
                base.Add(item);
                return item;
            }
        }

        [Serializable]
        public sealed class AttachmentCollection : ArrayList
        {
            public new BlogMLAttachment this[int index]
            {
                get { return base[index] as BlogMLAttachment; }
            }

            public void Add(BlogMLAttachment value)
            {
                base.Add(value);
            }
        }
    }
}
