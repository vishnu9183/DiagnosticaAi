namespace HealthcareApp.Models;

public class Prescription
{
    public int PrescriptionId { get; set; }
    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; }
    public string MedicationDetails { get; set; } // JSON or formatted string for medications
    public DateTime IssuedDate { get; set; }
}