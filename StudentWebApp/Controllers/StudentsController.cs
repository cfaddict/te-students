using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechElevator.Data;
using TechElevator.Models;

namespace TechElevator.Controllers
{
    public class StudentsController : Controller
    {
        private readonly TechElevatorContext _context;

        public StudentsController(TechElevatorContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string filterby = "", string filter = "")
        {
            ViewData["Tracks"] = getTracksDropDown();
            ViewData["Locations"] = getLocationsDropDown();

            var students = await _context.Students.Include(s => s.Track).Include(s => s.Location).ToListAsync();
            return View(students);
        }

        [Route("/students/track/{filter}", Name = "FilterByTrack")]
        public async Task<IActionResult> FilterByTrack(string filter)
        {
            ViewData["Locations"] = getLocationsDropDown();

            var students = await _context.Students
                .Include(s => s.Track)
                .Include(s => s.Location)
                .Where(s => s.Track.Name == filter)
                .ToListAsync();

            // show selected filter
            ViewData["Tracks"] = _context.Tracks
                .OrderBy(t => t.Name)
                .AsEnumerable()
                .Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString(), Selected = t.Name == filter }
            );

            return View("Views/Students/Index.cshtml", students);
        }

        [Route("/students/location/{filter}", Name = "FilterByLocation")]
        public async Task<IActionResult> FilterByLocation(string filter)
        {
            ViewData["Tracks"] = getTracksDropDown();
            ViewData["Locations"] = _context.Locations
                .OrderBy(t => t.Name)
                .AsEnumerable()
                .Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString(), Selected = t.Name == filter }
            );
            ViewData["SelectedLocation"] = filter;

            var students = await _context.Students
                    .Include(s => s.Track)
                    .Include(s => s.Location)
                    .Where(s => s.Location.Name == filter)
                    .ToListAsync();
            return View("Views/Students/Index.cshtml", students);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student student = await _context.Students
                .Include(s => s.Track)
                .Include(s => s.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["Tracks"] = getTracksDropDown();
            ViewData["Locations"] = getLocationsDropDown();
            return View();
        }


        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            var trackId = int.Parse(Request.Form["Track"]);
            Track track = _context.Tracks.Single(t => t.Id == trackId);
            student.Track = track;

            var locationId = int.Parse(Request.Form["Location"]);
            Location location = _context.Locations.Single(l => l.Id == locationId);
            student.Location = location;

            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Tracks"] = getTracksDropDown();
            ViewData["Locations"] = getLocationsDropDown();
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        private IEnumerable<SelectListItem> getTracksDropDown()
        {
            return _context.Tracks.OrderBy(t => t.Name).AsEnumerable().Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() });
        }

        private IEnumerable<SelectListItem> getLocationsDropDown()
        {
            return _context.Locations.OrderBy(t => t.Name).AsEnumerable().Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() });
        }
    }
}
