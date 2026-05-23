namespace ProjectTaskManagement.Application.UseCases.AuthCases.DTO;

public class RegisterResponseDto
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = "Registration successful. Please login.";
}
