using MovingScrewdriver.Web.Infrastructure;

namespace MovingScrewdriver.Web.ViewModels
{
    public class TagDetails
    {
        public string Title { get; set; }

        private string _slug;
        public string Slug
        {
            get { return _slug ?? (_slug = SlugConverter.TitleToSlug(Title)); }
        }
    }
}