using System;

namespace MovingScrewdriver.Web.Services.Model
{
    [Serializable]
    public class WpCategoryInfo
    {
        public string categoryId;

        public string categoryName;

        public string description;

        public string htmlUrl;

        public int parentId;

        public string rssUrl;
    }
}