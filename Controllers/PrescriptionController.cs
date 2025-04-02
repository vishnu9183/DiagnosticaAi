using HealthcareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthcareApp.Controllers;

[ApiController]
[Route("api/prescriptions")]
public class PrescriptionController : ControllerBase
{
    private readonly AppDbContext _context;

    public PrescriptionController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get prescriptions by appointment ID.
    /// </summary>
    /// <param name="appointmentId">The ID of the appointment.</param>
    /// <returns>A list of prescriptions associated with the appointment.</returns>
    [HttpGet("{appointmentId}")]
    public async Task<IActionResult> GetPrescriptionsByAppointmentId(int appointmentId)
    {
        if (appointmentId <= 0)
        {
            return BadRequest("Invalid Appointment ID. Appointment ID must be greater than 0.");
        }

        try
        {
            var prescriptions = await _context.Prescriptions
                .Where(p => p.AppointmentId == appointmentId)
                .ToListAsync();

            if (prescriptions == null || !prescriptions.Any())
            {
                return NotFound("No prescriptions found for the given appointment ID.");
            }

            return Ok(prescriptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching prescriptions: {ex.Message}");
            return StatusCode(500, "An error occurred while retrieving prescriptions. Please try again later.");
        }
    }
}
