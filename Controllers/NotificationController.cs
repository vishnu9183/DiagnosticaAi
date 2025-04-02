using HealthcareApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthcareApp.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationController : ControllerBase
{
    private readonly AppDbContext _context;

    public NotificationController(AppDbContext context)
    {
        _context = context;
    }

    // Get notifications for a specific patient
    [HttpGet("{patientId}")]
    public async Task<IActionResult> GetNotifications(int patientId)
    {
        var notifications = await _context.Notifications
            .Where(n => n.PatientId == patientId)
            .ToListAsync();
        return Ok(notifications);
    }

    // Mark a notification as read
    [HttpPut("{id}")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification == null)
            return NotFound();

        notification.IsRead = true;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}