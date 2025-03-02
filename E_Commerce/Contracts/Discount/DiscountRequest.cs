namespace E_Commerce.Contracts.Discount
{
    public record DiscountRequest(DateOnly StartAt , DateOnly EndAt , decimal NewPrice);
            //public int Id { get; set; }
            //public int CategoryId { get; set; }
            //public int ItemId { get; set; }
            //public DateOnly StartAt { get; set; }
            //public DateOnly EndAt { get; set; }
            //public decimal OldPrice { get; set; }
            //public decimal NewPrice { get; set; }
            //public bool IsActive => EndAt >= DateOnly.FromDateTime(DateTime.UtcNow);
            //public Category Category { get; set; } = default!;
            //public ICollection<Item> Items { get; set; } = new List<Item>();
}
