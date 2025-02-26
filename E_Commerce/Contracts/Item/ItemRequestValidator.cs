namespace E_Commerce.Contracts.Item
{
    public class ItemRequestValidator : AbstractValidator<ItemRequest>
    {
        public ItemRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(2, 100);
            RuleFor(x => x.Description)
                .NotEmpty();
            RuleFor(x => x.Price)
                .NotEmpty();
        }
    }
}
