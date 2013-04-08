using System;
using System.Web;
using MovingScrewdriver.Web.Infrastructure;
using MovingScrewdriver.Web.Infrastructure.AutoMapper.Resolvers;

namespace MovingScrewdriver.Web.Models
{
    public class PostReference
    {
        // if Id is: posts/1024, than domainId will be: 1024
        public string Id { get; set; }
        private string _title;
        public string Title
        {
            get { return _title; } 
            set { _title = HttpUtility.HtmlDecode(value); }
        }

        private int _domainId;
        public int DomainId
        {
            get
            {
                if (_domainId == 0)
                {
                    _domainId = RavenIdResolver.Resolve(Id);
                }
                    
                return _domainId;
            }
        }

        private string _slug;
        public string Slug
        {
            get { return _slug ?? (_slug = SlugConverter.TitleToSlug(Title)); }
            set { _slug = value; }
        }

        public DateTimeOffset PublishAt { get; set; }
    }
}