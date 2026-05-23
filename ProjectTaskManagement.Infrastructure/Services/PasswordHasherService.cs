using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ProjectTaskManagement.Infrastructure.Services;

public class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<User> _hasher = new();

    public string HashPassword(string password) =>
        _hasher.HashPassword(new User(), password);

    public bool VerifyPassword(string password, string passwordHash)
    {
        var result = _hasher.VerifyHashedPassword(new User(), passwordHash, password);
        return result == PasswordVerificationResult.Success;
    }
}
