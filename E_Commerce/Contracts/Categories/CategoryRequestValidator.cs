namespace E_Commerce.Contracts.Categories
{
    public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .Length(3, 100);
        }
    }
}
