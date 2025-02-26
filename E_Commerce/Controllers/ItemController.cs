using E_Commerce.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController(IItemServices itemServices) : ControllerBase
    {
        private readonly IItemServices _itemServices = itemServices;
        [HttpGet("{CatId:int}")]
        public async Task<IActionResult> GetAllItems(int CatId)
        {
            var result = await _itemServices.GetAllItemsAsync(CatId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPost("Add-New-Item/{Catid:int}")]
        public async Task<IActionResult> AddItem(int Catid, [FromForm] ItemRequest request)
        {
            var response = await _itemServices.AddItem(Catid, request);
            return response.IsSuccess ? Created() : response.ToProblem();
        }
        [HttpDelete("Delete-Item/{Catid:int}")]
        public async Task<IActionResult> DeleteItem(int Catid, int id , CancellationToken cancellationToken)
        {
            var response = await _itemServices.DeleteItem(Catid, id , cancellationToken);
            return response.IsSuccess ? NoContent() : response.ToProblem();
        }
    }
}
