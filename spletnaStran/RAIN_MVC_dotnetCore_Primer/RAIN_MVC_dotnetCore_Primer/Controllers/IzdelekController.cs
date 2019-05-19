using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vaja3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;


namespace Vaja3.Controllers
{
    public class IzdelekController : Controller
    {
        private readonly DBContext _context;

        public IzdelekController(DBContext context)
        {
            _context = context;
        }

        // GET: Izdelek
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Index()
        {
            ViewBag.oznaceni = HttpContext.Session.GetString("oznaceni")?.Split(",").Where(t => t.Length > 0).Select(t => int.Parse(t)).ToHashSet();
            return View(await _context.Novica.ToListAsync());
        }

        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Search(string searchString)
        {

            var novice = _context.Novica.Where(s => s.naziv.Contains(searchString));

            return View(novice);
        }

        public async Task<IActionResult> Oznaci(int? id) {
            HttpContext.Session.SetString("oznaceni", (HttpContext.Session.GetString("oznaceni") ?? "") + "," + id.ToString());
            return RedirectToAction("Index");
        }

        // GET: Izdelek/Details/5
        [Authorize(Roles ="Admin, User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izdelek = await _context.Novica
                .SingleOrDefaultAsync(m => m.id == id);
            if (izdelek == null)
            {
                return NotFound();
            }

            return View(izdelek);
        }

        // GET: Izdelek/Create
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("id,avtor,naziv,besedilo,datum")] Novica novica)
        {
            if (ModelState.IsValid)
            {
                _context.Add(novica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(novica);
        }

        // GET: Izdelek/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izdelek = await _context.Novica.SingleOrDefaultAsync(m => m.id == id);
            if (izdelek == null)
            {
                return NotFound();
            }
            return View(izdelek);
        }

        // POST: Izdelek/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("id,avtor,naziv,besedilo,datum")] Novica novica)
        {
            if (id != novica.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(novica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IzdelekExists(novica.id))
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
            return View(novica);
        }

        // GET: Izdelek/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izdelek = await _context.Novica
                .SingleOrDefaultAsync(m => m.id == id);
            if (izdelek == null)
            {
                return NotFound();
            }

            return View(izdelek);
        }

        // POST: Izdelek/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var izdelek = await _context.Novica.SingleOrDefaultAsync(m => m.id == id);
            _context.Novica.Remove(izdelek);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IzdelekExists(int? id)
        {
            return _context.Novica.Any(e => e.id == id);
        }
    }
}
