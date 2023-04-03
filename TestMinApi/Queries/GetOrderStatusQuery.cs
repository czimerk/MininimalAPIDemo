using AutoMapper;
using MediatR;
using TestMinApi.Dto;
using TestMinApi.Enums;
using TestMinApi.Helpers;

namespace TestMinApi.Queries
{
    public record GetOrderStatusQuery(Guid Id) : IRequest<OrderStatusDto>
    {
        public class GetOrderStatusHandler : IRequestHandler<GetOrderStatusQuery, OrderStatusDto>
        {
            private readonly IMapper _mapper;
            private readonly ILogger<GetOrderStatusHandler> _logger;

            public GetOrderStatusHandler(IMapper mapper, ILogger<GetOrderStatusHandler> logger)
            {
                _mapper = mapper;
                _logger = logger;
            }
            public async Task<OrderStatusDto> Handle(GetOrderStatusQuery request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Entered GetOrderStatusQuery Handle");
                return await Task.FromResult(new OrderStatusDto()
                {
                    ItemName = "ItemName",
                    Status = OrderStatus.Confirmed,
                    Updated = DateTime.Now
                });
            }
        }
    }


}
