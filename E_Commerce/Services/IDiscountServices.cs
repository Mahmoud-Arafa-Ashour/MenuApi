namespace E_Commerce.Services
{
    public interface IDiscountServices
    {
        Task<Result> AddDiscountAsync(int CategoryID, int ItemId, DiscountRequest request ,CancellationToken cancellationToken = default);
    }
}
