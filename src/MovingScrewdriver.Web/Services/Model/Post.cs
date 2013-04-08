using System;
using CookComputing.XmlRpc;

namespace MovingScrewdriver.Web.Services.Model
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class Post
    {
        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public string[] categories;

        public string[] custom_fields;

        public DateTime? dateCreated;

        public DateTime date_created_gmt;

        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public string description;

        public string link;

        public int mt_allow_comments;

        public int mt_allow_pings;

        public string mt_convert_breaks;

        public string mt_excerpt;


        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public string mt_keywords;

        public string mt_text_more;

        public string permalink;

        public string post_status;

        public string postid;

        public bool sticky;

        [XmlRpcMissingMapping(MappingAction.Error)]
        [XmlRpcMember(Description = "Required when posting.")]
        public string title;

        public string userid;

        public string wp_author_display_name;

        public string wp_author_id;

        public string wp_password;

        public string wp_slug;
    }
}