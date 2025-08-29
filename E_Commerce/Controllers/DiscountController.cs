namespace E_Commerce.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class DiscountController(IDiscountServices discountServices) : ControllerBase
{
    private readonly IDiscountServices _discountServices = discountServices;
    [HttpPost]
    public async Task<IActionResult> Add(int CategoryID, int ItemId, DiscountRequest request, CancellationToken cancellationToken = default)
    {
        var response =  await _discountServices.AddDiscountAsync(CategoryID, ItemId, request, cancellationToken);
        return response.IsSuccess ? Created() : response.ToProblem(); 
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(int categoryId, int itemId, CancellationToken cancellationToken = default)
    {
        var response = await _discountServices.DeleteDiscountAsync(categoryId, itemId , cancellationToken);
        return response.IsSuccess ? NoContent() : response.ToProblem();
    }
    [HttpPut]
    public async Task<IActionResult> Update(int categoryId, int itemId, DiscountRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _discountServices.UpdateDiscountAsync(categoryId , itemId , request, cancellationToken);
        return response.IsSuccess? NoContent() : response.ToProblem();
    }
}
