using MediatR;
using TestMinApi.Dto;
using TestMinApi.Helpers;

namespace TestMinApi.Queries
{
    public class GetOrdersQuery : IRequest<IEnumerable<OrderDto>>
    {
        public int Page { get; set; }
        public int PerPage { get; set; }

        public static ValueTask<GetOrdersQuery?> BindAsync(HttpContext context)
        {
            return BindingHelper.BindFromQueryAsync<GetOrdersQuery>(context);
        }

        public class GetOrdersRequestHandler : IRequestHandler<GetArticleQuery, IEnumerable<ArticleDto>>
        {
            public Task<IEnumerable<ArticleDto>> Handle(GetArticleQuery request, CancellationToken cancellationToken)
            {
                //request.PerPage, request.Page ...
                throw new NotImplementedException();
            }
        }
    }
}
