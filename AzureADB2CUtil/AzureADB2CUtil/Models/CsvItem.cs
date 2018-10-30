using AzureDay.WebApp.Database.Entities.Table;

namespace AzureADB2CUtil.Models
{
    public class CsvItem
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsPayed { get; set; }

        public string CouponCode { get; set; }
        public TicketType TicketType { get; set; }

        public int? WorkshopId { get; set; }

        public double Price { get; set; }

        public string PaymentType { get; set; }
    }
}