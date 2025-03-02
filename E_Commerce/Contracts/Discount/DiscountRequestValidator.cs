namespace E_Commerce.Contracts.Discount
{
    public class DiscountRequestValidator : AbstractValidator<DiscountRequest>
    {
        public DiscountRequestValidator()
        {
            RuleFor(x => x.StartAt)
                .NotEmpty();
            RuleFor(x => x.EndAt)
                .NotEmpty();
            RuleFor(x=>x.NewPrice)
                .NotEmpty();
        }
    }
}
