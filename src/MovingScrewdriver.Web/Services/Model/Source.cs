using CookComputing.XmlRpc;

namespace MovingScrewdriver.Web.Services.Model
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class Source
    {
        public string name;

        public string url;
    }
}