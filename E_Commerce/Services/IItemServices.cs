using E_Commerce.Contracts.Item;
namespace E_Commerce.Services
{
    public interface IItemServices
    {
        Task<Result<IEnumerable<ItemResponse>>> GetAllItemsAsync(int CatId);
        Task<Result<ItemResponse>> GetItemAsync(int Catid, int id);
        Task<Result> AddItem(int CatId, ItemRequest request);
        Task<Result> DeleteItem(int CatId, int id, CancellationToken cancellationToken);
        Task<Result<ItemResponse>> UpdateItemAsync(int catid, int id, ItemRequest request, CancellationToken cancellationToken);
    }
}
