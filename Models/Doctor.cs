namespace HealthcareApp.Models;

public class Doctor
{
    public int DoctorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Specialization { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    // Additional Attributes
    public List<DateTime> AvailableSlots { get; set; } = new List<DateTime>(); // Doctor's availability
    public bool IsApproved { get; set; } = true; // Admin approval status
    
    public string passwordhash { get; set; } // Store Hashed Password
}