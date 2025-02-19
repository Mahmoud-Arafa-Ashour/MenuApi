using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController(IItemServices itemServices) : ControllerBase
    {
        private readonly IItemServices _itemServices = itemServices;
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var result = await _itemServices.GetAllItemsAsync();
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id , CancellationToken cancellationToken)
        {
            var ItemResponse = await _itemServices.GetItemById(id, cancellationToken);
            return ItemResponse.IsSuccess ? Ok(ItemResponse) : ItemResponse.ToProblem();
        }
    }
}
