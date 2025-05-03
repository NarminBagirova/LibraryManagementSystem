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
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public IActionResult Index()
        {
            var books = _context.Books
                .Include(b => b.BookCategory)
                .Include(b => b.Publisher)
                .ToList();

            return View(books);
        }

        // GET: Books/Details/5
        public IActionResult Details(int id)
        {
            var book = _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookCategory)
                .Include(b => b.Publisher)
                .FirstOrDefault(b => b.Id == id);

            if (book == null) return NotFound();

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            var viewModel = GetBookFormViewModel();
            return View(viewModel);
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = GetBookFormViewModel(viewModel);
                return View(viewModel);
            }

            var book = new Book
            {
                Title = viewModel.Title,
                BookCategoryId = viewModel.BookCategoryId,
                PublisherId = viewModel.PublisherId,
                CreatedDate = DateTime.UtcNow.AddHours(4),
                UpdatedDate = DateTime.UtcNow.AddHours(4),
                BookAuthors = viewModel.SelectedAuthorIds
                    .Select(authorId => new BookAuthor { AuthorId = authorId })
                    .ToList()
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Books/Edit/5
        public IActionResult Edit(int id)
        {
            var book = _context.Books
                .Include(b => b.BookAuthors)
                .FirstOrDefault(b => b.Id == id);

            if (book == null) return NotFound();

            var viewModel = GetBookFormViewModel(new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                BookCategoryId = book.BookCategoryId,
                PublisherId = book.PublisherId,
                SelectedAuthorIds = book.BookAuthors.Select(ba => ba.AuthorId).ToList()
            });

            return View(viewModel);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, BookViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                viewModel = GetBookFormViewModel(viewModel);
                return View(viewModel);
            }

            var book = _context.Books
                .Include(b => b.BookAuthors)
                .FirstOrDefault(b => b.Id == id);

            if (book == null) return NotFound();

            book.Title = viewModel.Title;
            book.BookCategoryId = viewModel.BookCategoryId;
            book.PublisherId = viewModel.PublisherId;
            book.UpdatedDate = DateTime.UtcNow.AddHours(4);

            _context.BookAuthors.RemoveRange(book.BookAuthors);
            book.BookAuthors = viewModel.SelectedAuthorIds
                .Select(aid => new BookAuthor { AuthorId = aid, BookId = book.Id })
                .ToList();

            _context.Update(book);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Book updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Books/Delete/5
        public IActionResult Delete(int id)
        {
            var book = _context.Books
                .Include(b => b.BookCategory)
                .Include(b => b.Publisher)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .FirstOrDefault(b => b.Id == id);

            if (book == null) return NotFound();

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _context.Books.Find(id);

            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Book deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper method to generate form dropdowns
        private BookViewModel GetBookFormViewModel(BookViewModel viewModel = null)
        {
            viewModel ??= new BookViewModel();

            viewModel.BookCategories = _context.BookCategories
                .Select(bc => new SelectListItem { Value = bc.Id.ToString(), Text = bc.Name }).ToList();

            viewModel.Publishers = _context.Publishers
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();

            viewModel.Authors = _context.Authors
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.FullName }).ToList();

            return viewModel;
        }
    }
}
