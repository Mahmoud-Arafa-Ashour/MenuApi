
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
        public async Task<Result> AddItem(int CatId, ItemRequest request)
        {
            var category = await _dbContext.Categories.FindAsync(CatId);
            if (category is null)
            {
                return Result.Failure(CategoryErrors.EmptyCategory);
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
        public async Task<Result> DeleteItem(int CatId , int id , CancellationToken cancellationToken)
        {
            var IsExistedCategory = await _dbContext.Categories.AnyAsync(x=>x.Id == CatId);
            if (!IsExistedCategory) 
                return Result.Failure(CategoryErrors.EmptyCategory);
            var isExistedItem = await _dbContext.Items.AnyAsync(x=>(x.Id == id && x.CategoryId == CatId));
            if(!isExistedItem)
                return Result.Failure(ItemErrors.Emptyitem);
            var item = await _dbContext.Items.FirstOrDefaultAsync(x => (x.Id == id && x.CategoryId == CatId));
            _dbContext.Items.Remove(item!);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

    }
}
