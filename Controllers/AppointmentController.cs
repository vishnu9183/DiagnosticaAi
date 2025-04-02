using HealthcareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthcareApp.Controllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentController : ControllerBase
{
    private readonly AppDbContext _context;

    public AppointmentController(AppDbContext context)
    {
        _context = context;
    }

    // Get all appointments
    [HttpGet]
    public async Task<IActionResult> GetAllAppointments()
    {
        var appointments = await _context.Appointments.ToListAsync();
        return Ok(appointments);
    }

    // Schedule a new appointment
    [HttpPost]
    public async Task<IActionResult> AddAppointment([FromBody] Appointment appointment)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // 1) Add the new appointment
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        // 2) Reload with includes so we can access Doctor/Patient
        var savedAppointment = await _context.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.AppointmentId == appointment.AppointmentId);

        if (savedAppointment == null)
            return NotFound("Could not find the saved appointment.");

        // 3) Construct a note for health history
        //    For example, using the doctor's name and the appointment date/time:
        string newHistoryEntry = $"Appointment booked with Dr. " +
                                 $"{savedAppointment.Doctor?.FirstName} {savedAppointment.Doctor?.LastName} " +
                                 $"on {savedAppointment.AppointmentDateTime}";

        // 4) Append to the patient's HealthHistory list
        var patient = savedAppointment.Patient;
        if (patient == null)
            return NotFound("No patient found for this appointment.");

        if (patient.HealthHistory == null)
            patient.HealthHistory = new List<string>();

        patient.HealthHistory.Add(newHistoryEntry);

        // 5) Update the patient record
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();

        // Return the newly created appointment
        return CreatedAtAction(nameof(GetAllAppointments), new { id = appointment.AppointmentId }, savedAppointment);
    }
    
    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetAppointmentsByDoctorId(int doctorId)
    {
        // Query the appointments for the given doctor ID
        var appointments = await _context.Appointments
            .Where(a => a.DoctorId == doctorId)
            // Include related Doctor and Patient records
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .ToListAsync();

        return Ok(appointments);
    }
    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetAppointmentsByPatientId(int patientId)
    {
        // Query the appointments for the given doctor ID
        var appointments = await _context.Appointments
            .Where(a => a.PatientId == patientId)
            // Include related Doctor and Patient records
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .ToListAsync();

        return Ok(appointments);
    }
    
    [HttpPut("{appointmentId}/accept")]
    public async Task<IActionResult> AcceptAppointment(int appointmentId)
    {
        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment == null)
            return NotFound($"No appointment found with ID {appointmentId}.");

        // Update status to 'Accepted'
        appointment.Status = "Accepted";
        await _context.SaveChangesAsync();

        // Return updated appointment, or just return Ok
        return Ok(appointment);
    }
    
    // Update Patient Notes
    [HttpPut("{appointmentId}/patientnotes")]
    public async Task<IActionResult> UpdatePatientNotes(int appointmentId, [FromBody] NotesDto dto)
    {
        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment == null)
            return NotFound("Appointment not found.");

        appointment.PatientNotes = dto.Notes;
        await _context.SaveChangesAsync();
        return Ok(appointment);
    }
    
    // Update Doctor Notes
    [HttpPut("{appointmentId}/doctornotes")]
    public async Task<IActionResult> UpdateDoctorNotes(int appointmentId, [FromBody] NotesDto dto)
    {
        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment == null)
            return NotFound("Appointment not found.");

        appointment.DoctorNotes = dto.Notes;
        await _context.SaveChangesAsync();
        return Ok(appointment);
    }
    
    // Update AI Output
    [HttpPut("{appointmentId}/aioutput")]
    public async Task<IActionResult> UpdateAiOutput(int appointmentId, [FromBody] NotesDto dto)
    {
        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment == null)
            return NotFound("Appointment not found.");

        appointment.AiOutput = dto.Notes;
        await _context.SaveChangesAsync();
        return Ok(appointment);
    }
    
    // GET api/appointments/{appointmentId}
    [HttpGet("{appointmentId}")]
    public async Task<IActionResult> GetAppointmentById(int appointmentId)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

        if (appointment == null)
        {
            return NotFound($"No appointment found with ID {appointmentId}.");
        }

        return Ok(appointment);
    }
    
    [HttpDelete("{appointmentId}")]
    public async Task<IActionResult> DeleteAppointment(int appointmentId)
    {
        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment == null)
            return NotFound("Appointment not found.");

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
        return Ok("Appointment deleted successfully.");
    }
    
    // PUT api/appointments/{appointmentId}/reject
    [HttpPut("{appointmentId}/reject")]
    public async Task<IActionResult> RejectAppointment(int appointmentId)
    {
        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment == null)
            return NotFound("Appointment not found.");

        // Update status to 'Rejected'
        appointment.Status = "Rejected";
        await _context.SaveChangesAsync();
        return Ok(appointment);
    }
}
// DTO for sending note updates.
public class NotesDto
{
    public string Notes { get; set; }
}