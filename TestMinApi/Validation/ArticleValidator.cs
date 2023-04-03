using FluentValidation;
using TestMinApi.Dto;

namespace TestMinApi.Validation
{
    public class ArticleValidator : AbstractValidator<ArticleDto>
    {
        public ArticleValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(4);
            RuleFor(c => c.Unit).NotEmpty();
        }
    }

}
