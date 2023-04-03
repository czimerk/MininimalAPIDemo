using AutoMapper;
using MediatR;
using TestMinApi.Dto;
using TestMinApi.Helpers;
using TestMinApi.Services;

public class GetArticleByIdQuery : IRequest<ArticleDto>
{
    public Guid Id { get; set; }

    public static ValueTask<GetArticleByIdQuery?> BindAsync(HttpContext context)
    {
        return BindingHelper.BindFromQueryAsync<GetArticleByIdQuery?>(context);
    }

    public class GetArticleByIdHandler : IRequestHandler<GetArticleByIdQuery, ArticleDto?>
    {
        private readonly ArticleService _articleService;
        private readonly IMapper _mapper;

        public GetArticleByIdHandler(ArticleService articleService, IMapper mapper)
        {
            _articleService = articleService;
            _mapper = mapper;
        }
        public async Task<ArticleDto?> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
        {
            var article = await _articleService.GetArticleByIdAsync(request.Id);
            return _mapper.Map<ArticleDto>(article);
        }
    }
}