using ProjectTaskManagement.Application.UseCases.AuthCases.Commands;
using FluentValidation;

namespace ProjectTaskManagement.Application.UseCases.AuthCases.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(100);
    }
}
