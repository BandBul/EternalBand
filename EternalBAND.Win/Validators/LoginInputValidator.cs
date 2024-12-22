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
        }


    }
}
