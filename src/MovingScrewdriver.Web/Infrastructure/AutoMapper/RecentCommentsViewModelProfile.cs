using MovingScrewdriver.Web.Infrastructure.AutoMapper.Resolvers;
using MovingScrewdriver.Web.Models;
using MovingScrewdriver.Web.ViewModels;

namespace MovingScrewdriver.Web.Infrastructure.AutoMapper
{
    public class RecentCommentsViewModelProfile : AbstractProfile
    {
        protected override void Configure()
        {
            CreateMap<Post, RecentCommentViewModel>()
                .ForMember(x => x.PostId, o => o.MapFrom(m => RavenIdResolver.Resolve(m.Id)))
                .ForMember(x => x.PostTitle, o => o.MapFrom(m => m.Title))
                .ForMember(x => x.PostPublishAt, o => o.MapFrom(m => m.PublishAt))
                .ForMember(x => x.PostSlug, o => o.MapFrom(m => m.Slug))
                .ForMember(x => x.Author, o => o.Ignore())
                .ForMember(x => x.CommentId, o => o.Ignore())
                ;

            CreateMap<PostComments.Comment, RecentCommentViewModel>()
                .ForMember(x => x.PostId, o => o.Ignore())
                .ForMember(x => x.CommentId, o => o.MapFrom(x => x.Id))
                .ForMember(x => x.PostSlug, o => o.Ignore())
                .ForMember(x => x.PostTitle, o => o.Ignore())
                ;
        }
    }
}
