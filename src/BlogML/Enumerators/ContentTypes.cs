using System.Xml.Serialization ;

namespace BlogML
{
	public enum ContentTypes : short 
	{
		[XmlEnumAttribute("html")]
		Html = 1,
		[XmlEnumAttribute("xhtml")]
		Xhtml = 2,
		[XmlEnumAttribute("text")]
		Text = 3,
		[XmlEnumAttribute("base64")]
		Base64 = 4,
	}
}
