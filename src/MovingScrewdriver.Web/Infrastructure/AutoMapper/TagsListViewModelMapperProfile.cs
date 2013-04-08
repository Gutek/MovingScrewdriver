using AutoMapper;

namespace MovingScrewdriver.Web.Infrastructure.AutoMapper.Profiles
{
	public class TagsListViewModelMapperProfile : Profile
	{
		protected override void Configure()
		{
			Mapper.CreateMap<Tags_Count.ReduceResult, TagsListViewModel>()
				;
		}
	}
}