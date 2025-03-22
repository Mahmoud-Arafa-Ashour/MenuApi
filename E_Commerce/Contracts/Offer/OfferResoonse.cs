namespace E_Commerce.Contracts.Offer
{
    public record OfferResponse
        (
        string Name,
        string? Description,
        string? Photo,
        DateOnly StartDate,
        DateOnly EndDate,
        decimal Price,
        ICollection<OfferItemResponse> OfferItems
        );
}
