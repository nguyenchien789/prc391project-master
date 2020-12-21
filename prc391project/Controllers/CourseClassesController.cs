﻿using System;
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
    public class CourseClassesController : Controller
    {
        private readonly PRC391Context _context;

        public CourseClassesController(PRC391Context context)
        {
            _context = context;
        }

        // GET: CourseClasses
        public async Task<IActionResult> Index()
        {
            var name = HttpContext.Session.GetString("Test");
            var pRC391Context = _context.CourseClass
                .Include(c => c.Course).ThenInclude(c => c.Subject)
                .Include(c => c.Course).ThenInclude(c => c.Room)
                .Include(c => c.Course).ThenInclude(c => c.User)
                .Include(c => c.User).Where(c => c.UserId == name); ;
            return View(await pRC391Context.ToListAsync());
        }

        // GET: CourseClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseClass = await _context.CourseClass
                .Include(c => c.Course)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (courseClass == null)
            {
                return NotFound();
            }

            return View(courseClass);
        }

        // GET: CourseClasses/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId");
            return View();
        }

        // POST: CourseClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,UserId")] CourseClass courseClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", courseClass.CourseId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", courseClass.UserId);
            return View(courseClass);
        }

        // GET: CourseClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseClass = await _context.CourseClass.FindAsync(id);
            if (courseClass == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", courseClass.CourseId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", courseClass.UserId);
            return View(courseClass);
        }

        // POST: CourseClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,UserId")] CourseClass courseClass)
        {
            if (id != courseClass.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseClassExists(courseClass.CourseId))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseId", courseClass.CourseId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "UserId", courseClass.UserId);
            return View(courseClass);
        }

        // GET: CourseClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseClass = await _context.CourseClass
                .Include(c => c.Course)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (courseClass == null)
            {
                return NotFound();
            }

            return View(courseClass);
        }

        // POST: CourseClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseClass = await _context.CourseClass.FindAsync(id);
            _context.CourseClass.Remove(courseClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseClassExists(int id)
        {
            return _context.CourseClass.Any(e => e.CourseId == id);
        }
    }
}
