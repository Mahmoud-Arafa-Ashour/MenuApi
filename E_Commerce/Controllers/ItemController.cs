namespace E_Commerce.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class ItemController(IItemServices itemServices) : ControllerBase
{
    private readonly IItemServices _itemServices = itemServices;
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] RequestedFilters filters, int CatId , CancellationToken cancellationToken)
    {
        var result = await _itemServices.GetAllItemsAsync(filters, CatId , cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet]
    public async Task<IActionResult> Get(int CatId, int id)
    {
        var result = await _itemServices.GetItem(CatId, id);
        return result.Match(
            ItemResponse => Ok(ItemResponse),
            DiscountResponse => Ok(DiscountResponse),
            Error => Ok(ItemErrors.Emptyitem)
            );
    }
    [HttpPost]
    public async Task<IActionResult> Add(int Catid, [FromForm] ItemRequest request)
    {
        var response = await _itemServices.AddItem(Catid, request);
        return response.IsSuccess ? Created() : response.ToProblem();
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(int Catid, int id, CancellationToken cancellationToken)
    {
        var response = await _itemServices.DeleteItem(Catid, id, cancellationToken);
        return response.IsSuccess ? NoContent() : response.ToProblem();
    }
    [HttpPut]
    public async Task<IActionResult> Update(int Catid, int id, [FromForm]ItemRequest request, CancellationToken cancellationToken)
    {
        var response = await _itemServices.UpdateItemAsync(Catid, id, request, cancellationToken);
        return response.IsSuccess ? NoContent() : response.ToProblem();
    }
}
