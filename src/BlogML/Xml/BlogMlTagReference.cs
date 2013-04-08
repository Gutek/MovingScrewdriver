using System;
using System.Xml.Serialization;

namespace BlogML.Xml
{
    [Serializable]
    public sealed class BlogMLTagReference
    {
        private string _ref;

        [XmlAttribute("ref")]
        public string Ref
        {
            get { return this._ref; }
            set { this._ref = value; }
        }
    }
}