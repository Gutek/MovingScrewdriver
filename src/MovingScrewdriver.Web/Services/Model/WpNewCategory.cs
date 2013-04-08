using System;
using CookComputing.XmlRpc;

namespace MovingScrewdriver.Web.Services.Model
{
    [Serializable]
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class WpNewCategory
    {
        public string description;

        public string name;

        public int parent_id;

        public string slug;
    }
}