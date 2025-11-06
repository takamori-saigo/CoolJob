using CoolJob.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoolJob.Controllers
{
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly CoolJobContext _context;

        public ApplicationsController(CoolJobContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> My()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var applications = await _context.Applications
                .Include(a => a.Job)
                .ThenInclude(j => j.Employer)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.ApplyDate)
                .ToListAsync();

            return View(applications);
        }

        public async Task<IActionResult> Details(int id)
        {
            var application = await _context.Applications
                .Include(a => a.Job)
                .ThenInclude(j => j.Employer)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null) return NotFound();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (application.UserId != userId && !User.IsInRole("Employer"))
                return Forbid();

            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null) return NotFound();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (application.UserId != userId) return Forbid();

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(My));
        }
    }
}