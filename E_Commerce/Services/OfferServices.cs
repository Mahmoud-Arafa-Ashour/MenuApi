namespace E_Commerce.Services
{
    public class OfferServices(ApplicationDbContext dbContext) : IOfferServices
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<Result<IEnumerable<OfferResponse>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var response = await _dbContext.Offers.Include(x => x.OfferItems).ToListAsync(cancellationToken);
            var Offers = response.Adapt<IEnumerable<OfferResponse>>();
            return Result.Success(Offers);
        }

        public async Task<Result<OfferResponse>> GetAsync(int id, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Offers.Include(x => x.OfferItems).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (response == null)
                return Result.Failure<OfferResponse>(OfferErrors.EmptyOffer);
            var Offer = response.Adapt<OfferResponse>();
            return Result.Success(Offer);
        }
    }
}
