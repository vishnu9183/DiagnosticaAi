namespace HealthcareApp.Models;

public class Patient
{
    public int PatientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
    
    public List<string> UploadedLabReports { get; set; } = new List<string>(); // URLs for uploaded files
    public List<string> HealthHistory { get; set; } = new List<string>(); // Illness or condition history
    public List<Notification> Notifications { get; set; } = new List<Notification>(); // List of patient notifications
    
    public string passwordhash { get; set; } // Store Hashed Password
}