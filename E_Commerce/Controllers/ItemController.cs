namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class ItemController(IItemServices itemServices) : ControllerBase
    {
        private readonly IItemServices _itemServices = itemServices;
        [HttpGet("{CatId:int}")]
        public async Task<IActionResult> GetAll(int CatId)
        {
            var result = await _itemServices.GetAllItemsAsync(CatId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("Get-Item")]
        public async Task<IActionResult> Get(int CatId, int id)
        {
            var result = await _itemServices.GetItem(CatId, id);
            return result.Match(
                ItemResponse => Ok(ItemResponse),
                DiscountResponse => Ok(DiscountResponse),
                Error => Ok(ItemErrors.Emptyitem)
                );
        }
        [HttpPost("{Catid:int}")]
        public async Task<IActionResult> Add(int Catid, [FromForm] ItemRequest request)
        {
            var response = await _itemServices.AddItem(Catid, request);
            return response.IsSuccess ? Created() : response.ToProblem();
        }
        [HttpDelete("{Catid:int}")]
        public async Task<IActionResult> Delete(int Catid, int id, CancellationToken cancellationToken)
        {
            var response = await _itemServices.DeleteItem(Catid, id, cancellationToken);
            return response.IsSuccess ? NoContent() : response.ToProblem();
        }
        [HttpPut("{Catid:int}")]
        public async Task<IActionResult> Update(int Catid, int id, [FromForm]ItemRequest request, CancellationToken cancellationToken)
        {
            var response = await _itemServices.UpdateItemAsync(Catid, id, request, cancellationToken);
            return response.IsSuccess ? NoContent() : response.ToProblem();
        }
    }
}
