using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TestMinApi.Data.Domain;
using TestMinApi.Dto;
using TestMinApi.Extensions;
using TestMinApi.Helpers;
using TestMinApi.Services;

namespace TestMinApi.Queries
{
    public class GetArticleQuery : IRequest<IEnumerable<ArticleDto>>
    {
        public string? Name { get; set; }

        public double? PriceGreaterThan { get; set; }

        public double? PriceLessThan { get; set; }

        public static ValueTask<GetArticleQuery?> BindAsync(HttpContext context)
        {
            return BindingHelper.BindFromQueryAsync<GetArticleQuery>(context);
        }
    }

    public class GetArticleRequestHandler : IRequestHandler<GetArticleQuery, IEnumerable<ArticleDto>>
    {
        private readonly ArticleService _articleService;
        private readonly IMapper _mapper;

        public GetArticleRequestHandler(ArticleService articleService, IMapper mapper)
        {
            _articleService = articleService;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ArticleDto>> Handle(GetArticleQuery request,
            CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<FilterDto>(request);
            var articles = await _articleService.GetArticlesAsync(filter);
            return articles.Select(a => _mapper.Map<ArticleDto>(a)).ToList();
        }
    }


}
