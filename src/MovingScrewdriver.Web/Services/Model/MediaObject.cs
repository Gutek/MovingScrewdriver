using CookComputing.XmlRpc;

namespace MovingScrewdriver.Web.Services.Model
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class MediaObject
    {
        public byte[] bits;

        public string name;

        public string type;
    }
}