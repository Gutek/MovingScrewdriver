using System.Web.Mvc;
using AutoMapper;

namespace MovingScrewdriver.Web.Infrastructure.AutoMapper.Resolvers
{
	public class MvcHtmlStringConverter : TypeConverter<string, MvcHtmlString>
	{
		protected override MvcHtmlString ConvertCore(string source)
		{
			return MvcHtmlString.Create(source);
		}
	}
}