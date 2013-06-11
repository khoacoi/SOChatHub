using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace Web.ViewModels.Account.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserViewModel>
    {
        public RegisterUserValidator()
        {
            //RuleFor(x => x.Email).Must(ValidateEmailAddress).NotEmpty().WithMessage("Email is not valid, please check again.");
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.ConfirmPassword).NotEmpty();
            RuleFor(x => x.ConfirmPassword).Must(ValidateConfirmationPassword).WithMessage("Confirmation Password do not match.");
        }

        private bool ValidateConfirmationPassword(RegisterUserViewModel instance, string confirmPassword)
        {
            if (instance.Password != confirmPassword)
                return false;

            return true;
        }

        private bool ValidateEmailAddress(string emailAddress)
        {
            if (!emailAddress.Contains("@"))
                return false;

            return true;
        }
    }
}