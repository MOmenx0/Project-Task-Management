namespace ProjectTaskManagement.Application.UseCases.AuthCases.DTO;

public class AuthResponseDto
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
