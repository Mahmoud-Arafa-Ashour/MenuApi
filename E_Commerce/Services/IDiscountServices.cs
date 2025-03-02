namespace E_Commerce.Services
{
    public interface IDiscountServices
    {
        Task<Result> AddDiscountAsync(int CategoryID, int ItemId, DiscountRequest request ,CancellationToken cancellationToken = default);
        Task<Result> UpdateDiscountAsync(int categoryId, int itemId, DiscountRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteDiscountAsync(int categoryId, int itemId, CancellationToken cancellationToken = default);
        Task<Result<DiscountResponse>> GetItemWithDiscountAsync(int categoryId, int itemId, CancellationToken cancellationToken = default);
        Task<Result<List<DiscountResponse>>> GetAllItemsWithDiscountsAsync(int categoryId, CancellationToken cancellationToken = default);
    }
}
