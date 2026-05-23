using ProjectTaskManagement.Application.UseCases.AuthCases.Commands;
using FluentValidation;

namespace ProjectTaskManagement.Application.UseCases.AuthCases.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
