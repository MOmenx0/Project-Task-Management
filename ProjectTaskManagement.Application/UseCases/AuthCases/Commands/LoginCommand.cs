using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Application.Common.Models;
using ProjectTaskManagement.Application.Common.Specifications;
using ProjectTaskManagement.Application.UseCases.AuthCases.DTO;
using ProjectTaskManagement.Domain.Entities;
using MediatR;
using System.Net;

namespace ProjectTaskManagement.Application.UseCases.AuthCases.Commands;

public record LoginCommand(string Email, string Password) : IRequest<DataResponse<AuthResponseDto>>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, DataResponse<AuthResponseDto>>
{
    private readonly IunitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IunitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<DataResponse<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UsersRepository.GetOneAsync(
            new Specifications<User>(u => u.Email == request.Email.ToLowerInvariant()));

        if (user is null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        return new DataResponse<AuthResponseDto>
        {
            StatusCode = HttpStatusCode.OK,
            ResponseData = new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Token = _jwtTokenGenerator.GenerateToken(user)
            }
        };
    }
}
