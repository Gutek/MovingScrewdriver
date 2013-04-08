using CookComputing.XmlRpc;

namespace MovingScrewdriver.Web.Services.Model
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class Enclosure
    {
        public int length;

        public string type;

        public string url;
    }
}