namespace E_Commerce.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class CategoryController(ICategoryServices categoryServices, ApplicationDbContext dbContext, IOptions<JwtOptions> jwtOptions) : ControllerBase
{
    private readonly ICategoryServices _categoryServices = categoryServices;
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _categoryServices.GetAllCatrgoriresAsync(cancellationToken);
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> GetByid(int id, CancellationToken cancellationToken)
    {
        var response = await _categoryServices.GetCategoryById(id, cancellationToken);
        if (response == null)
            return NotFound();
        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> Add([FromForm] CategoryRequest request)
    {
        var result = await _categoryServices.CreateCategoryAsync(request);

        if (result.IsFailure)
        {
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }
    [HttpPut]
    public async Task<IActionResult> Update(int id,[FromForm] CategoryRequest request , CancellationToken cancellationToken)
    {
        var result = await _categoryServices.UpdateCategoryAsync(id,request , cancellationToken);
        return result.IsFailure ? result.ToProblem() : NoContent();
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await _categoryServices.DeleteCategoryAsync(id, cancellationToken);
        return response.IsFailure ? response.ToProblem() : Ok();
    }
}
