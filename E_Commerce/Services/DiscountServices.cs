using E_Commerce.Models;

namespace E_Commerce.Services
{
    public class DiscountServices(ApplicationDbContext dbContext) : IDiscountServices
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<Result> AddDiscountAsync(int CategoryID, int ItemId, DiscountRequest request, CancellationToken cancellationToken = default)
        {
            var IsExistedCategory = await _dbContext.Categories.AnyAsync(x => x.Id == CategoryID);
            if (!IsExistedCategory)
                return Result.Failure(CategoryErrors.EmptyCategory);

            var Item = await _dbContext.Items.FirstOrDefaultAsync(x => x.CategoryId == CategoryID && x.Id == ItemId);
            if (Item is null)
                return Result.Failure(ItemErrors.Emptyitem);

            if (request.NewPrice >= Item.Price)
                return Result.Failure(DiscountErrors.InvalidPrice);

            if (request.StartAt >= request.EndAt)
                return Result.Failure(DiscountErrors.InvalidDateRange);

            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await _dbContext.Discounts
                    .Where(x => x.CategoryId == CategoryID && x.ItemId == ItemId && !(x.EndAt >= DateOnly.FromDateTime(DateTime.UtcNow)))
                    .ExecuteDeleteAsync(cancellationToken);

                var IsExistedDiscount = await _dbContext.Discounts
                    .AnyAsync(x => x.CategoryId == CategoryID
                                && x.ItemId == ItemId
                                && x.EndAt >= request.StartAt, cancellationToken);

                if (IsExistedDiscount)
                    return Result.Failure(DiscountErrors.ExistingDiscount);

                var discount = new Discount()
                {
                    CategoryId = CategoryID,
                    ItemId = ItemId,
                    OldPrice = Item.Price,
                    NewPrice = request.NewPrice,
                    StartAt = request.StartAt,
                    EndAt = request.EndAt
                };

                _dbContext.Discounts.Add(discount);
                await _dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(new Error("DataBase.InvalidOperation", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }
        public async Task<Result> UpdateDiscountAsync(int categoryId, int itemId, DiscountRequest request, CancellationToken cancellationToken = default)
        {
            var isExistedCategory = await _dbContext.Categories.AnyAsync(x => x.Id == categoryId, cancellationToken);
            if (!isExistedCategory)
                return Result.Failure(CategoryErrors.EmptyCategory);

            var discount = await _dbContext.Discounts
                .Include(x => x.Item)  
                .FirstOrDefaultAsync(x => x.CategoryId == categoryId && x.ItemId == itemId, cancellationToken);

            if (discount is null)
                return Result.Failure(DiscountErrors.InvalidDiscount);

            if (request.NewPrice >= discount.Item.Price)
                return Result.Failure(DiscountErrors.InvalidPrice);

            if (request.StartAt >= request.EndAt)
                return Result.Failure(DiscountErrors.InvalidDateRange);

            discount.EndAt = request.EndAt;
            discount.NewPrice = request.NewPrice;
            _dbContext.Discounts.Update(discount);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        public async Task<Result> DeleteDiscountAsync(int categoryId, int itemId, CancellationToken cancellationToken = default)
        {
            var discount = await _dbContext.Discounts
                .FirstOrDefaultAsync(x => x.CategoryId == categoryId && x.ItemId == itemId, cancellationToken);

            if (discount is null)
                return Result.Failure(DiscountErrors.InvalidDiscount);

            _dbContext.Discounts.Remove(discount);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        public async Task<Result<DiscountResponse>> GetItemWithDiscountAsync(int categoryId, int itemId, CancellationToken cancellationToken = default)
        {
            var itemWithDiscount = await _dbContext.Items
                .Where(x => x.CategoryId == categoryId && x.Id == itemId)
                .Select(x => new
                {
                    x.Name,
                    x.Description,
                    x.Price,
                    x.ImagePath,
                    Discount = _dbContext.Discounts
                        .Where(d => d.CategoryId == categoryId && d.ItemId == itemId && d.EndAt >= DateOnly.FromDateTime(DateTime.UtcNow))
                        .Select(d => new { d.StartAt, d.EndAt, d.NewPrice })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (itemWithDiscount is null)
                return Result.Failure<DiscountResponse>(ItemErrors.Emptyitem);

            if (itemWithDiscount.Discount is null)
                return Result.Failure<DiscountResponse>(DiscountErrors.InvalidDiscount);

            var response = new DiscountResponse(
                itemWithDiscount.Name,
                itemWithDiscount.Description,
                itemWithDiscount.Discount.StartAt,
                itemWithDiscount.Discount.EndAt,
                itemWithDiscount.Price,
                itemWithDiscount.Discount.NewPrice,
                itemWithDiscount.ImagePath
            );

            return Result.Success(response);
        }
        public async Task<Result<List<DiscountResponse>>> GetAllItemsWithDiscountsAsync(int categoryId , CancellationToken cancellationToken = default)
        {
            // Check if the category exists
            var isExistedCategory = await _dbContext.Categories.AnyAsync(x => x.Id == categoryId, cancellationToken);
            if (!isExistedCategory)
                return Result.Failure<List<DiscountResponse>>(CategoryErrors.EmptyCategory);

            // Fetch all items along with their active discounts
            var itemsWithDiscounts = await _dbContext.Items
                .Where(x => x.CategoryId == categoryId)
                .Select(x => new
                {
                    x.Name,
                    x.Description,
                    x.Price,
                    x.ImagePath,
                    Discount = _dbContext.Discounts
                        .Where(d => d.CategoryId == categoryId && d.ItemId == x.Id && (d.EndAt >= DateOnly.FromDateTime(DateTime.UtcNow)))
                        .Select(d => new { d.StartAt, d.EndAt, d.NewPrice })
                        .FirstOrDefault()
                })
                .ToListAsync(cancellationToken);

            if (!itemsWithDiscounts.Any())
                return Result.Failure<List<DiscountResponse>>(ItemErrors.Emptyitem);

            // Map results to DiscountResponse list
            var responses = itemsWithDiscounts
                .Where(x => x.Discount != null) // Exclude items without an active discount
                .Select(x => new DiscountResponse(
                    x.Name,
                    x.Description,
                    x.Discount!.StartAt,
                    x.Discount!.EndAt,
                    x.Price,
                    x.Discount!.NewPrice,
                    x.ImagePath
                ))
                .ToList();

            if (!responses.Any())
                return Result.Failure<List<DiscountResponse>>(DiscountErrors.InvalidDiscount);

            return Result.Success(responses);
        }
    }
}
