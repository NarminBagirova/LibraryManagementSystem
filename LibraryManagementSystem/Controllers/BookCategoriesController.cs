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
    public class BookCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public BookCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.BookCategories
                .Select(x => new BookCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate,
                })
                .ToListAsync();

            return View(categories);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.BookCategories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null) return NotFound();

            var vm = new BookCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                CreatedDate = category.CreatedDate,
                UpdatedDate = category.UpdatedDate
            };

            return View(vm);
        }

        public IActionResult Create()
        {
            return View(new BookCategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new BookCategory
                {
                    Name = model.Name,
                    CreatedDate = DateTime.UtcNow.AddHours(4),
                    UpdatedDate = DateTime.UtcNow.AddHours(4)
                };

                _context.BookCategories.Add(category);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Category created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.BookCategories.FindAsync(id);
            if (category == null) return NotFound();

            var vm = new BookCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                CreatedDate = category.CreatedDate,
                UpdatedDate = category.UpdatedDate
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookCategoryViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var category = await _context.BookCategories.FindAsync(id);
                if (category == null) return NotFound();

                category.Name = model.Name;
                category.UpdatedDate = DateTime.UtcNow.AddHours(4);

                _context.Update(category);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Category updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.BookCategories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null) return NotFound();

            var vm = new BookCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                CreatedDate = category.CreatedDate,
                UpdatedDate = category.UpdatedDate
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.BookCategories.FindAsync(id);
            if (category != null)
            {
                _context.BookCategories.Remove(category);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Category deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookCategoryExists(int id)
        {
            return _context.BookCategories.Any(e => e.Id == id);
        }
    }
}