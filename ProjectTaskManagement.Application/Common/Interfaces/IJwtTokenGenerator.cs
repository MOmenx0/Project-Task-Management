using ProjectTaskManagement.Domain.Entities;

namespace ProjectTaskManagement.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
