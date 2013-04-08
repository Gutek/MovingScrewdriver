using System;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MovingScrewdriver.Web.Infrastructure.AutoMapper.Resolvers;
using MovingScrewdriver.Web.Models;
using MovingScrewdriver.Web.ViewModels;

namespace MovingScrewdriver.Web.Infrastructure.AutoMapper
{
    public class DefaultMappingProfile : Profile
    {
         protected override void Configure()
         {
             CreateMap<string, MvcHtmlString>().ConvertUsing<MvcHtmlStringConverter>();
             CreateMap<DateTimeOffset, DateTime>().ConvertUsing<DateTimeTypeConverter>();

             CreateMap<Post.SlugItem, TagDetails>();
             CreateMap<BlogOwner, AuthorDetails>()
                 .ForMember(x => x.FullName, o => o.MapFrom(m => m.FullName))
                 .ForMember(x => x.TwitterNick, o => o.MapFrom(m => m.Twitter))
                 ;

             CreateMap<Post, PostReference>()
                 .ForMember(x => x.Title, o => o.MapFrom(m => HttpUtility.HtmlDecode(m.Title)));
         }
    }
}