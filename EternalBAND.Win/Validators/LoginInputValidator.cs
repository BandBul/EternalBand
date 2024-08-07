using EternalBAND.DomainObjects.ApiContract;
using FluentValidation;

namespace EternalBAND.Win.Validators
{
    public class LoginInputValidator : AbstractValidator<LoginInputContract>
    {
        public LoginInputValidator() 
        {
            RuleFor(x => x.Username).NotEmpty()
                                    .WithMessage("User name is required.")
                                    .MaximumLength(100)
                                    .EmailAddress();
            RuleFor(x => x.Password).NotEmpty()
                                    .WithMessage("Password is required.")
                                    .MaximumLength(100);
            //RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.")
            //                          .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            //                          .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            //                          .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            //                          .Matches("[0-9]").WithMessage("Password must contain at least one numeric digit.")
            //                          .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
        }


    }
}
