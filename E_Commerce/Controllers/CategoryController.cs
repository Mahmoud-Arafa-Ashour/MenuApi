namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryServices categoryServices, ApplicationDbContext dbContext, IOptions<JwtOptions> jwtOptions) : ControllerBase
    {
        private readonly ICategoryServices _categoryServices = categoryServices;
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        [HttpGet("Displays-all-the-categories")]
        public async Task<IActionResult> GetAllCategries(CancellationToken cancellationToken)
        {
            var response = await _categoryServices.GetAllCatrgoriresAsync(cancellationToken);
            return Ok(response);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryByid(int id, CancellationToken cancellationToken)
        {
            var response = await _categoryServices.GetCategoryById(id, cancellationToken);
            if (response == null)
                return NotFound();
            return Ok(response);
        }
        [HttpPost("Create-category")]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryRequest request)
        {
            var result = await _categoryServices.CreateCategoryAsync(request);

            if (result.IsFailure)
            {
                return BadRequest(new { error = result.Error });
            }

            return Ok(result.Value);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id, CancellationToken cancellationToken)
        {
            var response = await _categoryServices.DeleteCategoryAsync(id, cancellationToken);
            return response.IsFailure ? response.ToProblem() : Ok();
        }
    }
}
