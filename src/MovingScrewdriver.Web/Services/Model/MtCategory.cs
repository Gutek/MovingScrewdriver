using System;
using CookComputing.XmlRpc;

namespace MovingScrewdriver.Web.Services.Model
{
    [Serializable]
    public class MtCategory
    {
        public string categoryId;

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string categoryName;

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public bool isPrimary;
    }
}