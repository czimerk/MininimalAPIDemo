using FluentValidation;
using MediatR;
using TestMinApi.Dto;
using TestMinApi.Services;
using TestMinApi.Validation;

namespace TestMinApi.Commands
{
    public record CreateArticleCommand(ArticleDto article) : IRequest<ArticleDto>
    {
       
    }

    public class CreateArticleHandler : HandlerValidator<ArticleDto>, IRequestHandler<CreateArticleCommand, ArticleDto>
    {
        private readonly ArticleService _articleService;

        public CreateArticleHandler(ArticleService articleService, IValidator<ArticleDto> validator):base(validator)
        {
            _articleService = articleService;
        }
        public async Task<ArticleDto> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            await HandleValidationAsync(request.article, cancellationToken);
            return await _articleService.CreateArticleAsync(request.article);
        }
    }
}
