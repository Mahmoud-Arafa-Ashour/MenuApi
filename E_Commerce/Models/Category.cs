namespace E_Commerce.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ImagePath { get; set; }
    public ICollection<Discount> Discounts { get; set; } = [];
    public ICollection<Item> items { get; set; } = [];
    public ICollection<OfferItem> OfferItems { get; set; } = []; // ✅ Fix here
}
