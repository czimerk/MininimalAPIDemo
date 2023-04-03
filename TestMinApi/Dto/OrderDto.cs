using TestMinApi.Data.Domain;

namespace TestMinApi.Dto
{
    public class OrderDto
    {
        public Status Status { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UserName { get; set; }
    }
}
