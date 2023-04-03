using TestMinApi.Enums;

namespace TestMinApi.Dto
{
    public class OrderStatusDto
    {
        public string ItemName { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime Updated { get; set; }
    }
}
