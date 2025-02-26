namespace E_Commerce.Services
{
    public class CategoryServices(ApplicationDbContext dbContext , IWebHostEnvironment environment) : ICategoryServices
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IWebHostEnvironment _environment = environment;

        public async Task<IEnumerable<CategoryResponse>> GetAllCatrgoriresAsync(CancellationToken cancellationToken)
        {
            var result = await _dbContext.Categories.ProjectToType<CategoryResponse>().ToListAsync();
            return result;
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
        public async Task<Result> DeleteCategoryAsync(int id , CancellationToken cancellationToken)
        {
            var isExisted =  _dbContext.Categories.Any(x=>x.Id == id);
            if(!isExisted) 
                return Result.Failure(new Error("Category.Not Found" , "This category is not Exixted" , StatusCodes.Status404NotFound));
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x=>x.Id == id);
            _dbContext.Categories.Remove(category!);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        public async Task<Result<CategoryResponse>> UpdateCategoryAsync(int id, CategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category is null)
                return Result.Failure<CategoryResponse>(new Error("Category.NotFound", "This category does not exist", StatusCodes.Status404NotFound));

            try
            {
                string imageUrl = category!.ImagePath!;

                if (request.image is not null)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                    string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.image.FileName)}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = File.Create(filePath))
                    {
                        await request.image.CopyToAsync(fileStream);
                    }

                    imageUrl = $"/uploads/{uniqueFileName}";
                }
                category.Name = request.Name;
                category.ImagePath = imageUrl;

                await _dbContext.SaveChangesAsync(cancellationToken);

                var response = new CategoryResponse(category.Name, imageUrl);
                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure<CategoryResponse>(new Error("Update.Invalid", ex.Message, StatusCodes.Status409Conflict));
            }
        }
    }
}
