namespace E_Commerce.Services
{
    public interface IOfferItemServices
    {
        Task<Result<OfferItemResponse>> Get(int offerId, int categoryId, int itemId, CancellationToken cancellationToken);
        Task<Result> Add(int offerId, int categoryId, int itemId, OfferItemRequest request, CancellationToken cancellationToken);
        Task<Result> Update(int offerId, int categoryId, int itemId, OfferItemRequest request, CancellationToken cancellationToken);
        Task<Result> Delete(int offerId, int categoryId, int itemId, CancellationToken cancellationToken);
    }
}
