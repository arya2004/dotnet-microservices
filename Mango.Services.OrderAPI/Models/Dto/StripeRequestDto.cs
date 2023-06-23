namespace Mango.Services.OrderAPI.Models.Dto
{
    public class StripeRequestDto
    {
        public string? StripeSessionUrl { get; set; }
        public string? StripeSessionId { get; set; }
        public string APprovedUrl { get; set; }
        public string CanceUrl { get; set; }
        public OrderHeaderDto OrderHeader { get; set; }
    }
}
