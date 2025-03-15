
using OneOf;
using static E_Commerce.Abstractions.Errors;

namespace E_Commerce.Services
{
    public class ItemServices(ApplicationDbContext dbContext , IWebHostEnvironment environment) : IItemServices
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IWebHostEnvironment _environment = environment;
        public async Task<Result<IEnumerable<ItemResponse>>> GetAllItemsAsync(int CatId)
        {
            var IsExistedCategory = await _dbContext.Categories.FindAsync(CatId);
            if (IsExistedCategory is null)
            {
                return Result.Failure<IEnumerable<ItemResponse>>(CategoryErrors.EmptyCategory);
            }
            var Items = await _dbContext.Items.Where(x => x.CategoryId == CatId).ProjectToType<ItemResponse>().ToListAsync();
            if (!Items.Any())
                return Result.Failure<IEnumerable<ItemResponse>>(ItemErrors.Emptyitem);
            return Result.Success<IEnumerable<ItemResponse>>(Items);
        }
        public async Task<Result<ItemResponse>> GetItemAsync(int Catid, int id)
        {
            var response = await _dbContext.Items.FirstOrDefaultAsync(x => x.CategoryId == Catid && x.Id == id);
            if (response is null)
                return Result.Failure<ItemResponse>(ItemErrors.Emptyitem);
            var item = response.Adapt<ItemResponse>();
            return Result.Success(item);
        }
        public async Task<Result> AddItem(int CatId, ItemRequest request)
        {
            var category = await _dbContext.Categories.FindAsync(CatId);
            if (category is null)
            {
                return Result.Failure(CategoryErrors.EmptyCategory);
            }
            var isExisteditem = await _dbContext.Items.AnyAsync(c => c.Name == request.Name && c.CategoryId == CatId);
            if (isExisteditem)
            {
                return Result.Failure<CategoryResponse>(new Error("Item.InvalidData", "This Item is already Existed", StatusCodes.Status409Conflict));
            }
            try
            {
                string? imageUrl = null;

                if (request.Image is not null)
                {
                    string uploadsFolder = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = File.Create(filePath))
                    {
                        await request.Image.CopyToAsync(fileStream);
                    }

                    imageUrl = $"/uploads/{uniqueFileName}";
                }

                var item = new Item()
                {
                    Name = request.Name,
                    CategoryId = CatId,
                    Description = request.Description,
                    Price = request.Price,
                    ImagePath = imageUrl
                };

                await _dbContext.Items.AddAsync(item);
                await _dbContext.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("Invalid Operation", ex.Message, StatusCodes.Status500InternalServerError));
            }

        }
        public async Task<Result> DeleteItem(int CatId, int id, CancellationToken cancellationToken)
        {
            var item = await _dbContext.Items
                .FirstOrDefaultAsync(x => x.Id == id && x.CategoryId == CatId, cancellationToken);

            if (item is null)
                return Result.Failure(ItemErrors.Emptyitem);

            try
            {
                if (!string.IsNullOrWhiteSpace(item.ImagePath))
                {
                    string uploadsFolder = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    string imagePath = Path.Combine(uploadsFolder, Path.GetFileName(item.ImagePath));

                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                _dbContext.Items.Remove(item);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("Delete.Invalid", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }
        public async Task<Result<ItemResponse>> UpdateItemAsync(int catid, int id, ItemRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var categoryExists = await _dbContext.Categories.AnyAsync(x => x.Id == catid, cancellationToken);
                if (!categoryExists)
                {
                    return Result.Failure<ItemResponse>(CategoryErrors.EmptyCategory);
                }

                var item = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == id && x.CategoryId == catid, cancellationToken);
                if (item is null)
                {
                    return Result.Failure<ItemResponse>(ItemErrors.Emptyitem);
                }

                if (string.IsNullOrWhiteSpace(request.Name) ||
                    string.IsNullOrWhiteSpace(request.Description) ||
                    request.Price <= 0)
                {
                    return Result.Failure<ItemResponse>(
                        new Error("InvalidRequest", "Invalid item details provided.", StatusCodes.Status400BadRequest));
                }

                string imageUrl = item.ImagePath ?? string.Empty;
                string uploadsFolder = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (request.Image is not null)
                {
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";
                    string newFilePath = Path.Combine(uploadsFolder, uniqueFileName);

                    if (!string.IsNullOrWhiteSpace(item.ImagePath))
                    {
                        string oldFilePath = Path.Combine(uploadsFolder, Path.GetFileName(item.ImagePath));
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }

                    await using var fileStream = File.Create(newFilePath);
                    await request.Image.CopyToAsync(fileStream, cancellationToken);

                    imageUrl = $"/uploads/{uniqueFileName}";
                }

                item.Name = request.Name;
                item.Description = request.Description;
                item.Price = request.Price;
                item.ImagePath = imageUrl;
                _dbContext.Items.Update(item);
                await _dbContext.SaveChangesAsync(cancellationToken);

                
                var response = new ItemResponse(item.Name, item.Description, item.Price, imageUrl);
                return Result.Success(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UpdateItemAsync Error] {ex.Message}");
                return Result.Failure<ItemResponse>(new Error("Update.Invalid", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<OneOf<ItemResponse, DiscountResponse, Error>> GetItem(int Catid, int id)
        {
            var response = await _dbContext.Items.FirstOrDefaultAsync(x => x.CategoryId == Catid && x.Id == id);
            if (response is null)
            {
                return ItemErrors.Emptyitem;
            }
            var discount = await _dbContext.Discounts.FirstOrDefaultAsync(x => x.CategoryId == Catid && x.ItemId == id && (x.EndAt >= DateOnly.FromDateTime(DateTime.UtcNow)));
            if (discount is null)
                return response.Adapt<ItemResponse>();
            var actualDiscount = new DiscountResponse(
                response.Name,
                response.Description,
                discount.StartAt,
                discount.EndAt,
                response.Price,
                discount.NewPrice,
                response.ImagePath
            );
            return actualDiscount;
        }
    }
}
