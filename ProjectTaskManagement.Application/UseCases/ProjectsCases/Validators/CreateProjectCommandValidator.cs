using ProjectTaskManagement.Application.UseCases.ProjectsCases.Commands;
using FluentValidation;

namespace ProjectTaskManagement.Application.UseCases.ProjectsCases.Validators;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);
    }
}
