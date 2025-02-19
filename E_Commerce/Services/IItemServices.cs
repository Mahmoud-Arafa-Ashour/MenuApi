using E_Commerce.Contracts.Item;

namespace E_Commerce.Services
{
    public interface IItemServices
    {
        Task<IEnumerable<ItemResponse>> GetAllItemsAsync();
        Task<Result<ItemResponse>> GetItemById(int id , CancellationToken cancellationToken = default);
    }
}
