using System;
using System.Xml.Serialization;

namespace BlogML.Xml
{
    [Serializable]
    public sealed class BlogMLTrackback : BlogMLNode
    {
        private string url;

        [XmlAttribute("url")]
        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        private string userName;
        private string userEmail;
        private BlogMLContent content = new BlogMLContent();
        private string userIp;

        [XmlAttribute("author")]
        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        [XmlAttribute("author-ip")]
        public string UserIp
        {
            get { return this.userIp; }
            set { this.userIp = value; }
        }

        [XmlAttribute("user-email")]
        public string UserEMail
        {
            get { return this.userEmail; }
            set { this.userEmail = value; }
        }

        [XmlElement("content")]
        public BlogMLContent Content
        {
            get { return this.content; }
            set { this.content = value; }
        }
    }
}
