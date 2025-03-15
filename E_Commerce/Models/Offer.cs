namespace E_Commerce.Models
{
    public sealed class Offer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Photo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public ICollection<OfferItem> OfferItems { get; set; } = [];
    }
}
