namespace E_Commerce.Models;

public class Discount
{
    public int CategoryId { get; set; }  // ✅ Composite PK (FK to Category)
    public int ItemId { get; set; }  // ✅ Composite PK (FK to Item)

    public DateOnly StartAt { get; set; }
    public DateOnly EndAt { get; set; }
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
    public Category Category { get; set; } = default!;
    public Item Item { get; set; } = default!;
}
