namespace HealthcareApp.Models;

public class Notification
{
    public int NotificationId { get; set; }
    public int PatientId { get; set; } // For patient-specific notifications
    public string Message { get; set; } // Notification text
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false; // For tracking read status
}