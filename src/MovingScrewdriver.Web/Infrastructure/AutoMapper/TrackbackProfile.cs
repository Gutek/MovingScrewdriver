using System;
using AutoMapper;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Models;
using MovingScrewdriver.Web.ViewModels;

namespace MovingScrewdriver.Web.Infrastructure.AutoMapper
{
    public class TrackbackProfile : Profile
    {
         protected override void Configure()
         {
             CreateMap<TrackbackInput, PostComments.Comment>()
                 .ForMember(x => x.UserHostAddress, o => o.MapFrom(m => GeneralUtils.GetClientIp()))
                 .ForMember(x => x.UserAgent, o => o.MapFrom(m => GeneralUtils.GetClientAgent()))
                 .ForMember(x => x.Type, o => o.MapFrom(m => CommentType.Trackback))
                 .ForMember(x => x.Author, o => o.MapFrom(m => m.blog_name))
                 .ForMember(x => x.Content, o => o.MapFrom(m => "Trackback od {0} - {1}".FormatWith(m.title, m.excerpt)))
                 .ForMember(x => x.Email, o => o.MapFrom(m => CommentType.Trackback.ToString()))
                 .ForAllMembers(x => x.Ignore())
                ;
         }
    }
}