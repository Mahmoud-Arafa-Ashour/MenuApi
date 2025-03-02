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
                    .Where(x => x.CategoryId == CategoryID && x.ItemId == ItemId && !x.IsActive)
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
            catch(Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(new Error("DataBase.InvalidOperation", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }
    }
}
