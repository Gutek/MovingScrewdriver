using System;
using AutoMapper;

namespace MovingScrewdriver.Web.Infrastructure.AutoMapper.Resolvers
{
	public class DateTimeTypeConverter : TypeConverter<DateTimeOffset, DateTime>
	{
		protected override DateTime ConvertCore(DateTimeOffset source)
		{
			return source.DateTime;
		}
	}
}
