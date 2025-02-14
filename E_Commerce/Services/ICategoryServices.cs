using E_Commerce.Abstractions;

namespace E_Commerce.Services
{
    public interface ICategoryServices
    {
        Task<IEnumerable<CategoryResponse>> GetAllCatrgoriresAsync(CancellationToken cancellationToken = default);
        Task<CategoryResponse> GetCategoryById(int id, CancellationToken cancellationToken = default);
        Task<Result<CategoryResponse>> CreateCategoryAsync(CategoryRequest request);
        Task<Result> DeleteCategoryAsync(int id, CancellationToken cancellationToken);

    }
}
