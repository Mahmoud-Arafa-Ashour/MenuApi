namespace E_Commerce.Contracts.Offer
{
    public record OfferResponse
        (
        string Name,
        string? Description,
        string? Photo,
        DateTime StartAt,
        DateTime EndAt,
        decimal Price,
        ICollection<OfferItemResponse> OfferItems
        );
}
