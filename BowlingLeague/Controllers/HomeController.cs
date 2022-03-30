using System.Linq;
using BowlingLeague.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BowlingLeague.Controllers
{
    public class HomeController : Controller
    {
        private IBowlingRepository _repo { get; set; }

        public HomeController(IBowlingRepository temp)
        {
            _repo = temp;
        }

        public IActionResult Index(string teamName)
        {
            var x = _repo.Bowlers
                .Include(b => b.Team)
                .Where(b => b.Team.TeamName == teamName || teamName == null)
                .OrderBy(b => b.Team.TeamName)
                .ToList();

            return View(x);
        }
    }
}
