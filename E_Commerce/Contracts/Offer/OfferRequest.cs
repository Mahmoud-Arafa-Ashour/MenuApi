namespace E_Commerce.Contracts.Offer
{
    public record OfferRequest
        (string Name ,
        string? Description,
        IFormFile? Photo,
        DateOnly StartDate,
        DateOnly EndDate,
        decimal Price
        );
}
