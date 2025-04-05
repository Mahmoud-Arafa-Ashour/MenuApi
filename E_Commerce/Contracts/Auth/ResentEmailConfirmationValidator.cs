﻿namespace E_Commerce.Contracts.Auth
{
    public class ResentEmailConfirmationValidator : AbstractValidator<ResentEmailConfirmationRequest>
    {
        public ResentEmailConfirmationValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");
        }
    }
}
