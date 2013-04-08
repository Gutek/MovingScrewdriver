using System.Web.Mvc;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.Infrastructure.AutoMapper.Resolvers;
using MovingScrewdriver.Web.Models;
using MovingScrewdriver.Web.ViewModels;

namespace MovingScrewdriver.Web.Infrastructure.AutoMapper
{
    public class PostViewModelProfile : AbstractProfile
    {
        protected override void Configure()
        {
            CreateMap<Post, PostViewModel.PostDetails>()
                .ForMember(x => x.Id, o => o.MapFrom(m => RavenIdResolver.Resolve(m.Id)))
                .ForMember(x => x.Slug, o => o.MapFrom(m => m.Slug))
                .ForMember(x => x.Description, o => o.MapFrom(m => DescriptionResolver.Resolve(MvcHtmlString.Create(m.Content), 200)))
                .ForMember(x => x.PublishedAt, o => o.MapFrom(m => m.PublishAt))
                .ForMember(x => x.IsCommentAllowed, o => o.MapFrom(m => m.AllowComments))
                .ForMember(x => x.Author, o => o.Ignore())
                ;

            CreateMap<PostComments.Comment, PostViewModel.Comment>()
                .ForMember(x => x.Content, o => o.MapFrom(m => MarkdownResolver.Resolve(m.Content)))
                .ForMember(x => x.EmailHash, o => o.MapFrom(m => EmailHashResolver.Resolve(m.Email)))
                .ForMember(x => x.IsImportant, o => o.MapFrom(m => m.Important))
                .ForMember(x => x.Url, o => o.MapFrom(m => UrlResolver.Resolve(m.Url)))
                .ForMember(x => x.CreatedAt, o => o.MapFrom(m => m.Created))
                ;

            CreateMap<CommentInput, PostComments.Comment>()
                .ForMember(x => x.Content, o => o.MapFrom(m => m.CommenterComment))
                .ForMember(x => x.Email, o => o.MapFrom(m => m.CommenterEmail))
                .ForMember(x => x.Type, o => o.MapFrom(m => CommentType.Comment))
                .ForMember(x => x.Created, o => o.MapFrom(m => ApplicationTime.Current))
                .ForMember(x => x.Author, o => o.MapFrom(m => m.CommenterName))
                .ForMember(x => x.Url, o => o.MapFrom(m => m.CommenterWebsite))
                .ForMember(x => x.UserAgent, o => o.MapFrom(m => GeneralUtils.GetClientAgent()))
                .ForMember(x => x.UserHostAddress, o => o.MapFrom(m => GeneralUtils.GetClientIp()))
                ;
        }
    }
}
