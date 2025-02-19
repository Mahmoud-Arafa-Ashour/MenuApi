
using static E_Commerce.Abstractions.Errors;

namespace E_Commerce.Services
{
    public class ItemServices(ApplicationDbContext dbContext) : IItemServices
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<IEnumerable<ItemResponse>> GetAllItemsAsync() =>
            await _dbContext.Items.ProjectToType<ItemResponse>().ToListAsync();

        public async Task<Result<ItemResponse>> GetItemById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.Items.FirstOrDefaultAsync(x=>x.Id == id);
            if(result == null) 
                return Result.Failure<ItemResponse>(ItemErrors.Emptyitem);
            var response = result.Adapt<ItemResponse>();
            return Result.Success(response);
        }
    }
}
