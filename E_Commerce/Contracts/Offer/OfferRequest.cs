namespace E_Commerce.Contracts.Offer
{
    public record OfferRequest
        (string Name ,
        string? Description,
        IFormFile Photo,
        DateTime StartDate,
        DateTime EndDate,
        decimal Price
        );
}
