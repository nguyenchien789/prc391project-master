using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prc391project.Models;

namespace prc391project.Controllers
{
    public class CoursesController : Controller
    {
        private readonly PRC391Context _context;

        public CoursesController(PRC391Context context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var name = HttpContext.Session.GetString("Test");
            var pRC391Context = _context.Course.Include(c => c.Room).Include(c => c.Subject).Include(c => c.User)
                .Include(c => c.CourseClass).Where(c => c.CourseClass .All(x => x.UserId != name));
            ViewData["userId"] = name;
            return View(await pRC391Context.ToListAsync());
        }

        // GET: Courses
        public async Task<IActionResult> TeacherIndex()
        {
            var name = HttpContext.Session.GetString("Test");
            var pRC391Context = _context.Course.Include(c => c.Room).Include(c => c.Subject).Include(c => c.User).Where(c => c.User.UserId == name);
            ViewData["userId"] = TempData["username"];
            return View(await pRC391Context.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Room)
                .Include(c => c.Subject)
                .Include(c => c.User)
                .Include(c => c.CourseClass).ThenInclude(c => c.User)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/StudentDetails/5
        public async Task<IActionResult> StudentDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Room)
                .Include(c => c.Subject)
                .Include(c => c.User)
                .Include(c => c.CourseClass).ThenInclude(c => c.User)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            var name = HttpContext.Session.GetString("Test");
            ViewData["RoomId"] = new SelectList(_context.Room.Where(c => c.IsUsed == false), "RoomId", "RoomId");
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "SubjectId");
            ViewData["UserId"] = new SelectList(_context.User.Where(c => c.UserId == name), "UserId", "UserId");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,UserId,SubjectId,RoomId,StartTime,EndTime")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                var editroom = await _context.Room.FindAsync(course.RoomId);
                editroom.IsUsed = true;
                _context.Update(editroom);
                await _context.SaveChangesAsync();
                return RedirectToAction("TeacherIndex", "Courses");
            }
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId", course.RoomId);
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "SubjectId", course.SubjectId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", course.UserId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var name = HttpContext.Session.GetString("Test");
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Room.Where(c => c.IsUsed == false || c.RoomId == course.RoomId), "RoomId", "RoomId", course.RoomId);
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "SubjectId", course.SubjectId);
            ViewData["UserId"] = new SelectList(_context.User.Where(c => c.UserId == name), "UserId", "UserId", course.UserId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,UserId,SubjectId,RoomId,StartTime,EndTime")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
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
            ViewData["RoomId"] = new SelectList(_context.Room, "RoomId", "RoomId", course.RoomId);
            ViewData["SubjectId"] = new SelectList(_context.Subject, "SubjectId", "SubjectId", course.SubjectId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", course.UserId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Room)
                .Include(c => c.Subject)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);

            var editroom = await _context.Room.FindAsync(course.RoomId);
            editroom.IsUsed = false;
            _context.Update(editroom);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(TeacherIndex));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}
