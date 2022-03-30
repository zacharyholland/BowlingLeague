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

        [HttpGet]
        public IActionResult CreateBowler()
        {
            //brings in teams list
            ViewBag.Teams = _repo.Teams.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateBowler(Bowler b)
        {

            if (ModelState.IsValid)
            {
                b.BowlerID = (_repo.Bowlers.Max(b => b.BowlerID)) + 1;
                _repo.CreateBowler(b);
                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.Teams = _repo.Teams.ToList();
                return View(b);
            }

        }

        [HttpGet]
        public IActionResult Edit(int bowlerid)
        {

            ViewBag.Teams = _repo.Teams.ToList();
            var bowler = _repo.Bowlers.Single(x => x.BowlerID == bowlerid);
            return View("CreateBowler", bowler);
        }

        [HttpPost]
        public IActionResult Edit(Bowler update)
        {
            _repo.SaveBowler(update);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Bowler delete)
        {
            _repo.DeleteBowler(delete);

            return RedirectToAction("Index");
        }
    }
}
