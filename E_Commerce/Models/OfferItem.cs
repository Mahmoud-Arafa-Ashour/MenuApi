namespace E_Commerce.Models;

public sealed class OfferItem
{
    public int OfferId { get; set; }
    public Offer Offer { get; set; } = default!;

    public int ItemId { get; set; }
    public Item Item { get; set; } = default!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;
    public int Quantity { get; set; }
}
