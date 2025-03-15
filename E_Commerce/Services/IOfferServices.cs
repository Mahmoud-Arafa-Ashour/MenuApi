namespace E_Commerce.Services
{
    public interface IOfferServices
    {
        Task<Result<IEnumerable<OfferResponse>>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result<OfferResponse>> GetAsync(int id, CancellationToken cancellationToken);
    }
}
