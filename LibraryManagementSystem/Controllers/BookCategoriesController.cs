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
            if (id == null)
            {
                return NotFound();
            }

            var bookCategory = await _context.BookCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookCategory == null)
            {
                return NotFound();
            }

            return View(bookCategory);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,CreatedDate,UpdatedDate")] BookCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bookCategory = new BookCategory
                {
                    Name = model.Name,
                    CreatedDate = DateTime.UtcNow.AddHours(4),
                    UpdatedDate = DateTime.UtcNow.AddHours(4)
                };

                _context.BookCategories.Add(bookCategory);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Category created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCategory = await _context.BookCategories.FindAsync(id);
            if (bookCategory == null)
            {
                return NotFound();
            }

            var viewModel = new BookCategoryViewModel
            {
                Id = bookCategory.Id,
                Name = bookCategory.Name,
                CreatedDate = bookCategory.CreatedDate,
                UpdatedDate = bookCategory.UpdatedDate
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,CreatedDate,UpdatedDate")] BookCategoryViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var bookCategory = await _context.BookCategories.FindAsync(id);
                    if (bookCategory == null)
                    {
                        return NotFound();
                    }

                    bookCategory.Name = model.Name;
                    bookCategory.UpdatedDate = DateTime.UtcNow.AddHours(4);

                    _context.Update(bookCategory);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookCategoryExists(model.Id.Value))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessMessage"] = "Category updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCategory = await _context.BookCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookCategory == null)
            {
                return NotFound();
            }

            return View(bookCategory);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookCategory = await _context.BookCategories.FindAsync(id);
            if (bookCategory != null)
            {
                _context.BookCategories.Remove(bookCategory);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Category deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool BookCategoryExists(int id)
        {
            return _context.BookCategories.Any(e => e.Id == id);
        }
    }
}