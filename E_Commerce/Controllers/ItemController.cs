namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemController(IItemServices itemServices) : ControllerBase
    {
        private readonly IItemServices _itemServices = itemServices;
        [HttpGet("Get-Items/{CatId:int}")]
        public async Task<IActionResult> GetAllItems(int CatId)
        {
            var result = await _itemServices.GetAllItemsAsync(CatId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("Get-Item")]
        public async Task<IActionResult> GetItem(int CatId, int id)
        {
            var result = await _itemServices.GetItem(CatId, id);
            return result.Match(
                ItemResponse => Ok(ItemResponse),
                DiscountResponse => Ok(DiscountResponse),
                Error => Ok(ItemErrors.Emptyitem)
                );
        }
        [HttpPost("Add-New-Item/{Catid:int}")]
        public async Task<IActionResult> AddItem(int Catid, [FromForm] ItemRequest request)
        {
            var response = await _itemServices.AddItem(Catid, request);
            return response.IsSuccess ? Created() : response.ToProblem();
        }
        [HttpDelete("Delete-Item/{Catid:int}")]
        public async Task<IActionResult> DeleteItem(int Catid, int id, CancellationToken cancellationToken)
        {
            var response = await _itemServices.DeleteItem(Catid, id, cancellationToken);
            return response.IsSuccess ? NoContent() : response.ToProblem();
        }
        [HttpPut("Update-Item/{Catid:int}")]
        public async Task<IActionResult> UpdateItem(int Catid, int id, [FromForm]ItemRequest request, CancellationToken cancellationToken)
        {
            var response = await _itemServices.UpdateItemAsync(Catid, id, request, cancellationToken);
            return response.IsSuccess ? NoContent() : response.ToProblem();
        }
    }
}
