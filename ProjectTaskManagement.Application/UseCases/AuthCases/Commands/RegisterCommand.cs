using ProjectTaskManagement.Application.Common.Behaviours;
using ProjectTaskManagement.Application.Common.Exceptions;
using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Common.Models;
using ProjectTaskManagement.Application.Common.Specifications;
using ProjectTaskManagement.Application.UseCases.AuthCases.DTO;
using ProjectTaskManagement.Domain.Entities;
using MediatR;
using System.Net;

namespace ProjectTaskManagement.Application.UseCases.AuthCases.Commands;

public record RegisterCommand(string Email, string Password) : IRequest<DataResponse<RegisterResponseDto>>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, DataResponse<RegisterResponseDto>>
{
    private readonly IunitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IunitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<DataResponse<RegisterResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existing = await _unitOfWork.UsersRepository.GetOneAsync(
            new Specifications<User>(u => u.Email == request.Email.ToLowerInvariant()));

        if (existing is not null)
        {
            throw new ValidationException(new[]
            {
                new ValidationEroor(nameof(request.Email), "Email is already registered.")
            });
        }

        var user = new User
        {
            Email = request.Email.ToLowerInvariant(),
            PasswordHash = _passwordHasher.HashPassword(request.Password)
        };

        await _unitOfWork.UsersRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new DataResponse<RegisterResponseDto>
        {
            StatusCode = HttpStatusCode.Created,
            ResponseData = new RegisterResponseDto
            {
                UserId = user.Id,
                Email = user.Email
            }
        };
    }
}
