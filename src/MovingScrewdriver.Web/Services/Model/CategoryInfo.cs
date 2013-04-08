using System;
using CookComputing.XmlRpc;

namespace MovingScrewdriver.Web.Services.Model
{
    [Serializable]
    public class CategoryInfo
    {
        public string categoryid;

        public string description;

        public string htmlUrl;

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string parentid;

        public string rssUrl;

        public string title;
    }
}