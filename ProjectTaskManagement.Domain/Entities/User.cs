namespace ProjectTaskManagement.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
