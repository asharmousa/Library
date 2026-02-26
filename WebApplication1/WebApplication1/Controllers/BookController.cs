using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BookController : Controller
    {
        private readonly MyDbContext _dbContext;
        public BookController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IActionResult showBookForStud()
        {
            var book = _dbContext.Books.ToList(); // show all the informaition in the database 
            return View(book);
        }

        public IActionResult ShowBookForAdmin() //Admin Dashboard
        {
            var book = _dbContext.Books.ToList(); // show all the informaition in the database 
            return View(book);
        }

        public IActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var query = _dbContext.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(b => b.BookTitle.Contains(searchTerm));
            }
            var results = await query.OrderBy(b => b.BookId).ToListAsync();
            TempData["searchTerm"] = searchTerm;
            return View("showBookForStud", results);
        }
        public IActionResult AddBook()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBook(Book book)
        {
           if(ModelState.IsValid != null)
            {
                book.BookStatus = "Available";
                book.IsBorrowed = false;
                _dbContext.Books.Add(book);
                _dbContext.SaveChanges();
                return RedirectToAction("ShowBookForAdmin");
            }

            return View(book);
        }
        public IActionResult UpdateBook(int id)
        {
            var Book = _dbContext.Books.FirstOrDefault(b => b.BookId == id);
            if (Book == null)
            {
                return NotFound();
            }
            return View(Book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateBook(Book book1)
        {
            if (ModelState.IsValid)
            {
                var wantedBook = _dbContext.Books.FirstOrDefault(b => b.BookId == book1.BookId);
                if (wantedBook == null)
                {
                    return NotFound();
                }
                else
                {
                    wantedBook.BookTitle = book1.BookTitle;
                    wantedBook.BookAuthor = book1.BookAuthor;
                    wantedBook.BookCategory = book1.BookCategory;
                    wantedBook.BookStatus = book1.BookStatus;
                    if (wantedBook.BookStatus == "Borrowed")
                    {
                        wantedBook.IsBorrowed = true;
                    }

                    _dbContext.SaveChanges();
                    return RedirectToAction("ShowBookForAdmin");
                }
            }
            return View();
        }
        public IActionResult DeleteBook(int id)
        {
            var Book = _dbContext.Books.FirstOrDefault(b => b.BookId == id);
            if (Book == null)
            {
                return NotFound();
            }
            _dbContext.Books.Remove(Book);
            _dbContext.SaveChanges();
            return RedirectToAction("ShowBookForAdmin");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RequestBorrow(int id)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.BookId == id);
            if(book == null)
            {
                return NotFound();
            }
            if(book.BookStatus == "Available")
            {
                book.BookStatus = "Pending";
                book.BorrowDate = DateOnly.FromDateTime(DateTime.Now);
                book.DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(14)); 
                book.ReturnDate = null;

                _dbContext.SaveChanges();
            }
            return RedirectToAction("ShowBookForStud");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApproveBorrow(int id)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            if (book.BookStatus == "Pending")
            {
                book.IsBorrowed = true;
                book.BookStatus = "Borrowed";
                _dbContext.SaveChanges();
            }
                return RedirectToAction("ShowBookForAdmin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RejectBorrow(int id)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            if (book.BookStatus == "Pending")
            {
                book.IsBorrowed = false;
                book.BookStatus = "Available";
                _dbContext.SaveChanges();
            }
            return RedirectToAction("ShowBookForAdmin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnBook(int id)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.BookId == id);
            if (book == null) return NotFound();

            if (book.BookStatus == "Borrowed")
            {
                book.BookStatus = "Available";
                book.IsBorrowed = false;
                book.ReturnDate = DateOnly.FromDateTime(DateTime.Now);

                _dbContext.SaveChanges();
            }

            return RedirectToAction("ShowBookForAdmin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CanselBorrow(int id)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.BookId == id);
            if (book == null) return NotFound();

            if (book.BookStatus == "Pending")
            {
                book.BookStatus = "Available";
                book.IsBorrowed = false;
                
                _dbContext.SaveChanges();
            }

            return RedirectToAction("ShowBookForStud");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
