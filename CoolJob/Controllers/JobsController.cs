using CoolJob.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoolJob.Controllers
{
    public class JobsController : Controller
    {
        private readonly CoolJobContext _context;

        public JobsController(CoolJobContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string search, JobType? type)
        {
            var query = _context.Jobs
                .Include(j => j.Employer)
                .Where(j => j.IsActive);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(j => j.Title.Contains(search) || 
                                        j.Description.Contains(search));

            if (type != null)
                query = query.Where(j => j.Type == type);

            ViewBag.JobTypes = typeof(JobType).GetEnumValues();
            return View(await query.ToListAsync());
        }


        public async Task<IActionResult> Details(int id)
        {
            var job = await _context.Jobs
                .Include(j => j.Employer)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null) return NotFound();

            return View(job);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Apply(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (_context.Applications.Any(a => a.JobId == id && a.UserId == userId))
                return RedirectToAction("Details", new { id });

            var application = new Application
            {
                JobId = id,
                UserId = userId,
                ApplyDate = DateTime.Now,
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id });
        }
    }
}