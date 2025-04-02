namespace HealthcareApp.Models;
using System.Text.Json.Serialization;

public class Appointment
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public string Status { get; set; } 

    // Navigation properties:
    public Doctor? Doctor { get; set; }
    public Patient? Patient { get; set; }
    public string Notes { get; set; } 
    
    // Additional Attributes (optional, so they are not required when creating an appointment)
    public string? DoctorNotes { get; set; }  // General notes or instructions (e.g. from the doctor)
    public string? PatientNotes { get; set; }        // Notes added by the patient
    public string? AiOutput { get; set; }            // AI-generated output for file uploads
}