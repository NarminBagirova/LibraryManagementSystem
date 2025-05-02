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
    public class AuthorsController : Controller
    {
        private readonly AppDbContext _context;

        public AuthorsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await _context.Authors
                .Include(a => a.AuthorContact)
                .Select(a => new AuthorViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Surname = a.Surname,
                    Email = a.AuthorContact != null ? a.AuthorContact.Email : null,
                    PhoneNumber = a.AuthorContact != null ? a.AuthorContact.PhoneNumber : null
                })
                .ToListAsync();

            return View(authors);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var author = await _context.Authors
                .Include(a => a.AuthorContact)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return NotFound();

            var vm = new AuthorViewModel
            {
                Id = author.Id,
                Name = author.Name,
                Surname = author.Surname,
                Email = author.AuthorContact?.Email,
                PhoneNumber = author.AuthorContact?.PhoneNumber
            };

            return View(vm);
        }

        public IActionResult Create()
        {
            return View(new AuthorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var author = new Author
            {
                Name = model.Name,
                Surname = model.Surname,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                AuthorContact = new AuthorContact
                {
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                }
            };

            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Author created successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var author = await _context.Authors
                .Include(a => a.AuthorContact)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return NotFound();

            var vm = new AuthorViewModel
            {
                Id = author.Id,
                Name = author.Name,
                Surname = author.Surname,
                Email = author.AuthorContact?.Email,
                PhoneNumber = author.AuthorContact?.PhoneNumber
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuthorViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var author = await _context.Authors
                .Include(a => a.AuthorContact)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return NotFound();

            author.Name = model.Name;
            author.Surname = model.Surname;
            author.UpdatedDate = DateTime.UtcNow;

            if (author.AuthorContact == null)
            {
                author.AuthorContact = new AuthorContact
                {
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };
            }
            else
            {
                author.AuthorContact.Email = model.Email;
                author.AuthorContact.PhoneNumber = model.PhoneNumber;
                author.AuthorContact.UpdatedDate = DateTime.UtcNow;
            }

            _context.Update(author);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Author updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var author = await _context.Authors
                .Include(a => a.AuthorContact)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return NotFound();

            var vm = new AuthorViewModel
            {
                Id = author.Id,
                Name = author.Name,
                Surname = author.Surname,
                Email = author.AuthorContact?.Email,
                PhoneNumber = author.AuthorContact?.PhoneNumber
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors
                .Include(a => a.AuthorContact)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return NotFound();

            if (author.AuthorContact != null)
                _context.AuthorContacts.Remove(author.AuthorContact);

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Author deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
