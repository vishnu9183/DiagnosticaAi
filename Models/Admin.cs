namespace HealthcareApp.Models;

public class Admin
{
    public int AdminId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } // Store hashed passwords in production
}