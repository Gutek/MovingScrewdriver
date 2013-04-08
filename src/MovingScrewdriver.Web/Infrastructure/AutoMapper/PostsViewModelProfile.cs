using System.Web.Mvc;
using AutoMapper;
using MovingScrewdriver.Web.Infrastructure.AutoMapper.Resolvers;
using MovingScrewdriver.Web.Models;
using MovingScrewdriver.Web.ViewModels;

namespace MovingScrewdriver.Web.Infrastructure.AutoMapper
{
    public class PostsViewModelProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Post, PostsViewModel.PostSummary>()
                .ForMember(x => x.Id, o => o.MapFrom(m => RavenIdResolver.Resolve(m.Id)))
                .ForMember(x => x.Author, o => o.Ignore())
                .ForMember(x => x.Description, o => o.MapFrom(m => DescriptionResolver.ResolveWithTrailling(MvcHtmlString.Create(m.Content), 500)))
                .ForMember(x => x.PublishedAt, o => o.MapFrom(m => m.PublishAt))
                ;
        }
    }
}
