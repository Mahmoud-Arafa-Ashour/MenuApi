namespace E_Commerce.Services
{
    public interface ICategoryServices
    {
        Task<Result<PaginatedData<CategoryResponse>>> GetAllCatrgoriresAsync(RequestedFilters filters, CancellationToken cancellationToken);
        Task<CategoryResponse> GetCategoryById(int id, CancellationToken cancellationToken = default);
        Task<Result<CategoryResponse>> CreateCategoryAsync(CategoryRequest request);
        Task<Result> DeleteCategoryAsync(int id, CancellationToken cancellationToken);
        Task<Result<CategoryResponse>> UpdateCategoryAsync(int id, CategoryRequest request, CancellationToken cancellationToken);
    }
}
