namespace E_Commerce.Contracts.Item
{
    public record ItemResponse(string Name, string Description , decimal Price , string? ImagePath);
}
