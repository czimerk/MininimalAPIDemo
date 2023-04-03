using AutoMapper;
using TestMinApi.Data.Domain;
using TestMinApi.Dto;
using TestMinApi.Queries;

namespace MiraIntro.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Article, ArticleDto>();

            CreateMap<GetArticleQuery, FilterDto>().ReverseMap();
        }
    }
}
