using DocumentFormat.OpenXml.Office2010.Excel;

namespace E_Commerce.Services
{
    public class OfferServices(ApplicationDbContext dbContext , IWebHostEnvironment environment) : IOfferServices
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IWebHostEnvironment _environment = environment;
        public async Task<Result> Add(OfferRequest request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
            {
                return Result.Failure(new Error("InvalidData", "Invalid Offer data", StatusCodes.Status409Conflict));
            }
            var isExistedOffer = await _dbContext.Offers.AnyAsync(c => c.Name == request.Name);
            if (isExistedOffer)
            {
                return Result.Failure(new Error("Offers.InvalidData", "This Offer is already Existed", StatusCodes.Status409Conflict));
            }
            try
            {
                string? imageUrl = null;  // Initialize as null
                if (request.Photo is not null)  // Only process if an image is provided
                {
                    string uploadsFolder = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Photo.FileName)}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = File.Create(filePath))
                    {
                        await request.Photo.CopyToAsync(fileStream);
                    }
                    imageUrl = $"/uploads/{uniqueFileName}";  // Assign uploaded image URL
                }
                var offer = new Offer
                {
                    Name = request.Name,
                    Description = request.Description,
                    Photo = imageUrl,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Price = request.Price
                };
                _dbContext.Offers.Add(offer);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("DataBase.InvalidOperation", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        public async Task<Result<IEnumerable<OfferResponse>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var response = await _dbContext.Offers
                .Where(o => o.StartDate <= o.EndDate)
                .Select(o => new OfferResponse(
                    o.Name,
                    o.Description,
                    o.Photo,
                    o.StartDate,
                    o.EndDate,
                    o.Price,
                    o.OfferItems.Select(oi => new OfferItemResponse(oi.Item.Name, oi.Quantity)).ToList()
                )).ToListAsync(cancellationToken);
            if(response.Count ==0)
                return Result.Failure<IEnumerable<OfferResponse>>(OfferErrors.NotValidOffers);
            return Result.Success<IEnumerable<OfferResponse>>(response);
        }

        public async Task<Result<OfferResponse>> GetAsync(int id, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Offers
                .Where(o => o.Id == id)
                .Select(o => new OfferResponse(
                    o.Name,
                    o.Description,
                    o.Photo,
                    o.StartDate,
                    o.EndDate,
                    o.Price,
                    o.OfferItems.Select(oi => new OfferItemResponse(oi.Item.Name, oi.Quantity)).ToList()
                ))
                .FirstOrDefaultAsync(cancellationToken);

            if (response == null)
                return Result.Failure<OfferResponse>(OfferErrors.EmptyOffer);

            return Result.Success(response);
        }
        public async Task<Result> Delete(int id, CancellationToken cancellationToken)
        {
            var IsExixtedOffer = await _dbContext.Offers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (IsExixtedOffer is null)
                return Result.Failure(OfferErrors.EmptyOffer);
            try
            {
                if (!string.IsNullOrWhiteSpace(IsExixtedOffer.Photo))
                {
                    string uploadsFolder = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    string imagePath = Path.Combine(uploadsFolder, Path.GetFileName(IsExixtedOffer.Photo));

                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                _dbContext.Offers.Remove(IsExixtedOffer);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("Delete.Invalid", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }
        public async Task<Result> Update(int id, OfferRequest request, CancellationToken cancellationToken)
        {
            var offer = await _dbContext.Offers.FindAsync(id);
            if (offer is null)
                return Result.Failure(OfferErrors.EmptyOffer);
            try
            {
                string uploadFileName = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if(!Directory.Exists(uploadFileName))
                {
                    Directory.CreateDirectory(uploadFileName);
                }
                string imageUrl = offer.Photo!;
                if (request.Photo is not null)
                {
                    string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Photo.FileName)}";
                    string filePath = Path.Combine(uploadFileName, uniqueFileName);
                    if (!string.IsNullOrWhiteSpace(offer.Photo))
                    {
                        string oldFilePath = Path.Combine(uploadFileName, Path.GetFileName(offer.Photo));
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }
                    using (var fileStream = File.Create(filePath))
                    {
                        await request.Photo.CopyToAsync(fileStream, cancellationToken);
                    }
                    imageUrl = $"/uploads/{uniqueFileName}";
                }
                offer.Name = request.Name;
                offer.Photo = imageUrl;
                offer.Description = request.Description;
                offer.StartDate = request.StartDate;
                offer.EndDate = request.EndDate;
                offer.Price = request.Price;
                _dbContext.Offers.Update(offer);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure<CategoryResponse>(new Error("Update.Invalid", ex.Message, StatusCodes.Status500InternalServerError));
            }
        }
    }
}
