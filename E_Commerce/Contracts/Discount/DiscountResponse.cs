namespace E_Commerce.Contracts.Discount
{
    public record DiscountResponse(
    string Name,
    string Description,
    DateOnly StartAt,
    DateOnly EndAt,
    decimal OldPrice,
    decimal NewPrice,
    string? ImagePath);
}
