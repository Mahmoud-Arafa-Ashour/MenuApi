namespace E_Commerce.Contracts.Offer
{
    public class OfferRequestValidator : AbstractValidator<OfferRequest>
    {
        public OfferRequestValidator()
        {
            RuleFor(RuleFor => RuleFor.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x =>x.StartDate).NotEmpty().WithMessage("Start Date is required");
            RuleFor(x => x.EndDate).NotEmpty().WithMessage("End Date is required");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required");
            RuleFor(x=> x.StartDate).LessThan(RuleFor => RuleFor.EndDate).WithMessage("Start Date should be less than or equal to End Date");
            RuleFor(x =>x.EndDate).GreaterThan(RuleFor => RuleFor.StartDate).WithMessage("End Date should be greater than or equal to Start Date");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price should be greater than 0");
        }
    }
}
