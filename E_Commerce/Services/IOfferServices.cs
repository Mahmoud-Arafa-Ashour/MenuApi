namespace E_Commerce.Services
{
    public interface IOfferServices
    {
        Task<Result> Add(OfferRequest request, CancellationToken cancellationToken);
        Task<Result<IEnumerable<OfferResponse>>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result<OfferResponse>> GetAsync(int id, CancellationToken cancellationToken);
        Task<Result> Delete(int id, CancellationToken cancellationToken);
        Task<Result> Update(int id, OfferRequest request, CancellationToken cancellationToken);
    }
}
