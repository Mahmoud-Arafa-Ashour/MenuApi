namespace E_Commerce.Contracts.Item
{
    public record ItemRequest(string Name, string Description , decimal Price , IFormFile? Image);
}
//public int Id { get; set; }
//public string Name { get; set; } = string.Empty;
//public string Description { get; set; } = string.Empty;
//public decimal Price { get; set; }
//public string? ImagePath { get; set; }
//public int CategoryId { get; set; }
//public Category Category { get; set; } = default!;
