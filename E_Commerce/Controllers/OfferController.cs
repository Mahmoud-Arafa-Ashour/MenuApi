namespace E_Commerce.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class OfferController(IOfferServices offerServices) : ControllerBase
{
    private readonly IOfferServices _offerServices = offerServices;
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var offers = await _offerServices.GetAllAsync(cancellationToken);
        return offers.IsSuccess ? Ok(offers.Value) : offers.ToProblem();
    }
    [HttpGet]
    public async Task<IActionResult> Get(int id , CancellationToken cancellationToken)
    {
        var offers = await _offerServices.GetAsync(id,cancellationToken);
        return offers.IsSuccess ? Ok(offers.Value) : offers.ToProblem();
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(int id , CancellationToken cancellationToken)
    {
        var resopnse = await _offerServices.Delete(id, cancellationToken);
        return resopnse.IsSuccess ? NoContent() : resopnse.ToProblem();
    }
    [HttpPut]
    public async Task<IActionResult> Update(int id, [FromForm]OfferRequest request, CancellationToken cancellationToken)
    {
        var response = await _offerServices.Update(id, request, cancellationToken);
        return response.IsSuccess ? NoContent() : response.ToProblem();
    }
    [HttpPost]
    public async Task<IActionResult> Add([FromForm]OfferRequest request, CancellationToken cancellationToken)
    {
        var response = await _offerServices.Add(request, cancellationToken);
        return response.IsSuccess ? Created() : response.ToProblem();
    }
}
