using HealthcareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthcareApp.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientController : ControllerBase
{
    private readonly AppDbContext _context;

    public PatientController(AppDbContext context)
    {
        _context = context;
    }

    // Get all patients
    [HttpGet]
    public async Task<IActionResult> GetAllPatients()
    {
        var patients = await _context.Patients.ToListAsync();
        return Ok(patients);
    }

    // Add a new patient
    [HttpPost]
    public async Task<IActionResult> AddPatient([FromBody] Patient patient)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAllPatients), new { id = patient.PatientId }, patient);
    }

    // Login endpoint (POST method)
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        Console.WriteLine("Hitting function");
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email == request.Email && p.passwordhash == request.Password);

        if (patient == null)
            return Unauthorized("Invalid email or password.");

        return Ok(new { message = "Login successful", patientId = patient.PatientId });
    }
    
    // Delete a patient
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null)
            return NotFound();

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientById(int id)
    {
        // Attempt to find the patient by the given ID
        var patient = await _context.Patients.FindAsync(id);

        // If no matching patient is found, return 404 (Not Found)
        if (patient == null)
            return NotFound($"No patient found with ID {id}.");

        // Otherwise, return OK (200) with the patient's data
        return Ok(patient);
    }
}

// Model for login requests
public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}