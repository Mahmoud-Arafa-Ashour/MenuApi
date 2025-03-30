using static E_Commerce.Abstractions.Errors;

namespace E_Commerce.Services
{
    public class CategoryServices(ApplicationDbContext dbContext , IWebHostEnvironment environment) : ICategoryServices
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IWebHostEnvironment _environment = environment;

        public async Task<Result<PaginatedData<CategoryResponse>>> GetAllCatrgoriresAsync(RequestedFilters filters,CancellationToken cancellationToken)
        {
            var result =  _dbContext.Categories.ProjectToType<CategoryResponse>();
            var paginatedData = await PaginatedData<CategoryResponse>.CreateAsync(result, filters.PageNumber, filters.PageSize, cancellationToken);
            return Result.Success<PaginatedData<CategoryResponse>>(paginatedData);
        }
        public async Task<CategoryResponse> GetCategoryById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (result == null) return null!;
            return result.Adapt<CategoryResponse>();
        }
        public async Task<Result<CategoryResponse>> CreateCategoryAsync(CategoryRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
            {
                return Result.Failure<CategoryResponse>(new Error("InvalidData", "Invalid category data", StatusCodes.Status409Conflict));
            }
            var isExistedCategory = await _dbContext.Categories.AnyAsync(c => c.Name == request.Name);
            if (isExistedCategory)
            {
                return Result.Failure<CategoryResponse>(new Error("Categories.InvalidData", "This Category is already Existed", StatusCodes.Status409Conflict));
            }
            try
            {
                string? imageUrl = null;  // Initialize as null

                if (request.image is not null)  // Only process if an image is provided
                {
                    string uploadsFolder = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.image.FileName)}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = File.Create(filePath))
                    {
                        await request.image.CopyToAsync(fileStream);
                    }

                    imageUrl = $"/uploads/{uniqueFileName}";  // Assign uploaded image URL
                }

                var category = new Category
                {
                    Name = request.Name,
                    ImagePath = imageUrl // Can be null if no image was uploaded
                };

                _dbContext.Categories.Add(category);
                await _dbContext.SaveChangesAsync();

                var response = new CategoryResponse(category.Name, imageUrl);

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure<CategoryResponse>(new Error("DatabaseError", ex.Message, StatusCodes.Status409Conflict));
            }
        }
        public async Task<Result> DeleteCategoryAsync(int id, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (category is null)
                return Result.Failure(CategoryErrors.EmptyCategory);

            try
            {
                if (!string.IsNullOrWhiteSpace(category.ImagePath))
                {
                    string uploadsFolder = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    string imagePath = Path.Combine(uploadsFolder, Path.GetFileName(category.ImagePath));

                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("Delete.Invalid", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<Result<CategoryResponse>> UpdateCategoryAsync(int id, CategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category is null)
                return Result.Failure<CategoryResponse>(CategoryErrors.EmptyCategory);

            try
            {
                
                if (string.IsNullOrWhiteSpace(request.Name))
                    return Result.Failure<CategoryResponse>(new Error("InvalidRequest", "Category name cannot be empty", StatusCodes.Status400BadRequest));

                string uploadsFolder = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string imageUrl = category.ImagePath!;

                if (request.image is not null)
                {
                    string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.image.FileName)}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    
                    if (!string.IsNullOrWhiteSpace(category.ImagePath))
                    {
                        string oldFilePath = Path.Combine(uploadsFolder, Path.GetFileName(category.ImagePath));
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }

                    
                    using (var fileStream = File.Create(filePath))
                    {
                        await request.image.CopyToAsync(fileStream, cancellationToken);
                    }

                    imageUrl = $"/uploads/{uniqueFileName}";
                }

                category.Name = request.Name;
                category.ImagePath = imageUrl;
                _dbContext.Categories.Update(category);
                await _dbContext.SaveChangesAsync(cancellationToken);

                var response = new CategoryResponse(category.Name, imageUrl);
                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure<CategoryResponse>(new Error("Update.Invalid", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }
    }
}
