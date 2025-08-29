namespace E_Commerce.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImagePath { get; set; }
    public int CategoryId { get; set; }

    public Category Category { get; set; } = default!;
    public Discount? Discount { get; set; }
    public ICollection<OfferItem> OfferItems { get; set; } = []; // ✅ Fix here
}
