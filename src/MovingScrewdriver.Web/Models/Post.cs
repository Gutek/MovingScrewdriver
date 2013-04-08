using System;
using System.Collections.Generic;
using MovingScrewdriver.Web.Infrastructure;
using Raven.Imports.Newtonsoft.Json;

namespace MovingScrewdriver.Web.Models
{
    public class Post
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        private string _slug;
        
        public string Slug
        {
            get { return _slug ?? (_slug = SlugConverter.TitleToSlug(Title)); }
            set { _slug = value; }
        }

        public string LegacyUniqueId { get; set; }
        public string LegacySlug { get; set; }

        public ICollection<SlugItem> Tags { get; set; }
        public ICollection<SlugItem> Categories { get; set; }

        public string AuthorId { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }
        public DateTimeOffset PublishAt { get; set; }
        
        public bool IsDeleted { get; set; }
        public bool AllowComments { get; set; }

        public int CommentsCount { get; set; }
        public string CommentsId { get; set; }

        [JsonIgnore]
        public IEnumerable<string> TagsAsSlugs
        {
            get
            {
                if (Tags == null)
                {
                    yield break;
                }
                    
                foreach (var tag in Tags)
                {
                    yield return tag.Slug;
                }
            }
        }
        [JsonIgnore]
        public IEnumerable<string> CategoriesAsSlugs
        {
            get
            {
                if (Categories == null)
                {
                    yield break;
                }

                foreach (var category in Categories)
                {
                    yield return category.Slug;
                }
            }
        }

        public class SlugItem
        {
            private string _slug;

            public string Title { get; set; }

            public string Slug
            {
                get { return _slug ?? (_slug = SlugConverter.TitleToSlug(Title)); }
                set { _slug = value; }
            }
        }
    }
}