namespace E_Commerce.Models;

public sealed class Offer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Photo { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Price { get; set; }
    public ICollection<OfferItem> OfferItems { get; set; } = [];
}
