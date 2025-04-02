using HealthcareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthcareApp.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorController : ControllerBase
{
    private readonly AppDbContext _context;

    public DoctorController(AppDbContext context)
    {
        _context = context;
    }

    // Get all approved doctors
    [HttpGet]
    public async Task<IActionResult> GetAllDoctors()
    {
        var doctors = await _context.Doctors.Where(d => d.IsApproved).ToListAsync();
        return Ok(doctors);
    }

    // Add a new doctor
    [HttpPost]
    public async Task<IActionResult> AddDoctor([FromBody] Doctor doctor)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAllDoctors), new { id = doctor.DoctorId }, doctor);
    }

    // Update a doctor
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDoctor(int id, [FromBody] Doctor doctor)
    {
        if (id != doctor.DoctorId)
            return BadRequest();

        _context.Entry(doctor).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Delete a doctor
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDoctor(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null)
            return NotFound();

        _context.Doctors.Remove(doctor);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Doctor login endpoint (POST)
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Email == request.Email && d.passwordhash == request.Password);

        if (doctor == null)
            return Unauthorized("Invalid email or password.");

        return Ok(new { message = "Login successful", doctorId = doctor.DoctorId });
    }
    
    // NEW: Get doctors by specialty with a fallback if no matches are found.
    [HttpGet("specialty/{specialty}")]
    public async Task<IActionResult> GetDoctorsBySpecialty(string specialty)
    {
        Console.WriteLine("Fetch doctors hit");
        // Query approved doctors whose Specialization contains the given specialty string (case-insensitive)
        var doctors = await _context.Doctors
            .Where(d => d.IsApproved && d.Specialization.ToLower().Contains(specialty.ToLower()))
            .ToListAsync();

        // If no matching doctors are found, use a fallback
        if (doctors == null || doctors.Count == 0)
        {
            // Example: fallback to "General Practitioner" doctors (adjust as needed)
            doctors = await _context.Doctors
                .Where(d => d.IsApproved && d.Specialization.ToLower().Contains("General"))
                .ToListAsync();
        }

        return Ok(doctors);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDoctorById(int id)
    {
        // Attempt to find the doctor by the given ID
        var doctor = await _context.Doctors.FindAsync(id);

        // If no matching doctor is found, return 404 (Not Found)
        if (doctor == null)
            return NotFound($"No doctor found with ID {id}.");

        // Otherwise, return OK (200) with the doctor’s data
        return Ok(doctor);
    }
}

// You can place this in a shared file if you want to reuse for Patients/Admin, etc.
public class login
{
    public string Email { get; set; }
    public string Password { get; set; }
}
