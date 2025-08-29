namespace E_Commerce.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class OfferItemController(IOfferItemServices offerItemServices) : ControllerBase
{
    private readonly IOfferItemServices _offerItemServices = offerItemServices;
    [HttpGet]
    public async Task<IActionResult> Get(int offerId, int categoryId, int itemId , CancellationToken cancellationToken)
    {
        var result = await _offerItemServices.Get(offerId, categoryId, itemId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost]
    public async Task<IActionResult> Add(int offerId , int categoryId, int itemId , OfferItemRequest request, CancellationToken cancellationToken)
    {
        var result = await _offerItemServices.Add(offerId, categoryId, itemId, request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut]
    public async Task<IActionResult> Update(int offerId, int categoryId, int itemId, OfferItemRequest request, CancellationToken cancellationToken)
    {
        var result = await _offerItemServices.Update(offerId, categoryId, itemId, request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
