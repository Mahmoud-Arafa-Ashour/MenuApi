
using DocumentFormat.OpenXml.Wordprocessing;

namespace E_Commerce.Services
{
    public class OfferItemServices(ApplicationDbContext dbContext) : IOfferItemServices
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<Result> Add(int offerId, int categoryId, int itemId, OfferItemRequest request, CancellationToken cancellationToken)
        {
            var isExistedOffer = await _dbContext.Offers.AnyAsync(x => x.Id == offerId , cancellationToken);
            if (!isExistedOffer)
                return Result.Failure(OfferErrors.EmptyOffer);
            var isExistedCategory = await _dbContext.Categories.AnyAsync(x => x.Id == categoryId, cancellationToken);
            if (!isExistedOffer)
                return Result.Failure(CategoryErrors.EmptyCategory); 
            var isExistedItem = await _dbContext.Items.AnyAsync(x => x.Id == itemId, cancellationToken);
            if (!isExistedOffer)
                return Result.Failure(ItemErrors.Emptyitem);
            var offerItem = new OfferItem
            {
                OfferId = offerId,
                CategoryId = categoryId,
                ItemId = itemId,
                Quantity = request.Quantity,
            };
            _dbContext.OfferItems.Add(offerItem);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        public async Task<Result> Update(int offerId, int categoryId, int itemId, OfferItemRequest request, CancellationToken cancellationToken)
        {
            var isExistedOffer = await _dbContext.Offers.AnyAsync(x => x.Id == offerId, cancellationToken);
            if (!isExistedOffer)
                return Result.Failure(OfferErrors.EmptyOffer);
            var isExistedCategory = await _dbContext.Categories.AnyAsync(x => x.Id == categoryId, cancellationToken);
            if (!isExistedOffer)
                return Result.Failure(CategoryErrors.EmptyCategory);
            var isExistedItem = await _dbContext.Items.AnyAsync(x => x.Id == itemId, cancellationToken);
            if (!isExistedOffer)
                return Result.Failure(ItemErrors.Emptyitem);
            var isExistedOfferItem = 
                await _dbContext.OfferItems.FirstOrDefaultAsync(x => x.OfferId == offerId && x.CategoryId == categoryId && x.ItemId == itemId, cancellationToken);
            if (isExistedOfferItem == null)
                return Result.Failure(OfferItemErrors.EmptyOfferItem);
            isExistedOfferItem.Quantity = request.Quantity;
            _dbContext.OfferItems.Update(isExistedOfferItem);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result<OfferItemResponse>> Get(int offerId, int categoryId, int itemId, CancellationToken cancellationToken)
        {
            var response = await _dbContext.OfferItems.
                Include(x => x.Item).
                FirstOrDefaultAsync(x => x.OfferId == offerId && x.CategoryId == categoryId && x.ItemId == itemId, cancellationToken);
            if(response == null)
                return Result.Failure<OfferItemResponse>(OfferItemErrors.EmptyOfferItem);
            var offer = new OfferItemResponse
                (
                Name: response.Item.Name,
                Quantity: response.Quantity
                );
            return Result.Success(offer);
        }

        public async Task<Result> Delete(int offerId, int categoryId, int itemId, CancellationToken cancellationToken)
        {
            var isExistedOffer = await _dbContext.Offers.AnyAsync(x => x.Id == offerId, cancellationToken);
            if (!isExistedOffer)
                return Result.Failure(OfferErrors.EmptyOffer);
            var isExistedCategory = await _dbContext.Categories.AnyAsync(x => x.Id == categoryId, cancellationToken);
            if (!isExistedOffer)
                return Result.Failure(CategoryErrors.EmptyCategory);
            var isExistedItem = await _dbContext.Items.AnyAsync(x => x.Id == itemId, cancellationToken);
            if (!isExistedOffer)
                return Result.Failure(ItemErrors.Emptyitem);
            var isExistedOfferItem =
                await _dbContext.OfferItems.FirstOrDefaultAsync(x => x.OfferId == offerId && x.CategoryId == categoryId && x.ItemId == itemId, cancellationToken);
            if (isExistedOfferItem == null)
                return Result.Failure(OfferItemErrors.EmptyOfferItem);
            _dbContext.OfferItems.Remove(isExistedOfferItem);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
