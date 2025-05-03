using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Controllers
{
    public class PublishersController : Controller
    {
        private readonly AppDbContext _context;

        public PublishersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Publishers
        public async Task<IActionResult> Index()
        {
            var publishers = await _context.Publishers
                .Select(x => new PublisherViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate
                })
                .ToListAsync();

            return View(publishers);
        }

        // GET: Publishers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var publisher = await _context.Publishers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (publisher == null) return NotFound();

            return View(publisher);
        }

        // GET: Publishers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publishers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PublisherViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var publisher = new Publisher
            {
                Name = model.Name,
                CreatedDate = DateTime.UtcNow.AddHours(4),
                UpdatedDate = DateTime.UtcNow.AddHours(4)
            };

            _context.Add(publisher);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Publisher created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Publishers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null) return NotFound();

            var viewModel = new PublisherViewModel
            {
                Id = publisher.Id,
                Name = publisher.Name,
                CreatedDate = publisher.CreatedDate,
                UpdatedDate = publisher.UpdatedDate
            };

            return View(viewModel);
        }

        // POST: Publishers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PublisherViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid) return View(model);

            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null) return NotFound();

            publisher.Name = model.Name;
            publisher.UpdatedDate = DateTime.UtcNow.AddHours(4);

            _context.Update(publisher);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Publisher updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Publishers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var publisher = await _context.Publishers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (publisher == null) return NotFound();

            return View(publisher);
        }

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Publisher deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(int id)
        {
            return _context.Publishers.Any(e => e.Id == id);
        }
    }
}
