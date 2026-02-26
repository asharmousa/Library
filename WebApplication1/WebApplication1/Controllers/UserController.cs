using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using OfficeOpenXml;
using System;
using System.Reflection.Metadata.Ecma335;
using WebApplication1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ClosedXML.Excel;


namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        private readonly MyDbContext _dbContext;

        public UserController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserTable user , IFormFile profilePic)
        {
            if (ModelState.IsValid)
            {
                if (user.Role?.ToLower() == "student")
                {
                    if (!user.Email.EndsWith("@ses.yu.edu.jo"))
                    {
                        ModelState.AddModelError("Email", "Student email must end with @ses.yu.edu.jo");
                        return View(user);
                    }
                }

                if (profilePic != null && profilePic.Length > 0)
                {
                    var fileName = Path.GetFileName(profilePic.FileName);
                    var filePath = Path.Combine("wwwroot/images/profiles", fileName); // change 

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        profilePic.CopyTo(stream);
                    }

                    user.ProfilePicture = "/images/profiles/" + fileName;
                }

                _dbContext.UserTables.Add(user);
                _dbContext.SaveChanges();

                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("Role", user.Role ?? "");

                return RedirectToAction("showBookForStud" , "Book");
            }
            return View(user);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password, string role)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and Password are required.");
                return View();
            }

            // Student login
            if (role == "Student")
            {
                if (!email.EndsWith("@ses.yu.edu.jo", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("email", "Student email must end with @ses.yu.edu.jo");
                    return View();
                }

                var stud = _dbContext.UserTables
                    .FirstOrDefault(s => s.Email == email && s.Password == password && s.Role == "Student");

                if (stud == null)
                {
                    ModelState.AddModelError("", "Invalid student credentials.");
                    return View();
                }

                HttpContext.Session.SetInt32("UserId", stud.UserId);
                HttpContext.Session.SetString("Role", stud.Role);

                return RedirectToAction("showBookForStud","Book"); //login 
            }
            if (role == "Admin")
            {
                var admin = _dbContext.UserTables
                    .FirstOrDefault(a => a.Email == email && a.Password == password && a.Role == "Admin");

                if (admin == null)
                {
                    ModelState.AddModelError("", "Invalid admin credentials.");
                    return View();
                }

                HttpContext.Session.SetInt32("UserId", admin.UserId);
                HttpContext.Session.SetString("Role", admin.Role);

                return RedirectToAction("showBookForAdmin", "Book");
            }

            ModelState.AddModelError("role", "Invalid role selected.");
            return View();
        }

        public IActionResult ProfileManagement()
        {
            var userId = HttpContext.Session.GetInt32("UserId");  
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var stud = _dbContext.UserTables.FirstOrDefault(u => u.UserId == userId.Value);
            stud.FirstName = HttpContext.Session.GetString("FirstName");
            stud.LastName = HttpContext.Session.GetString("LastName");
            return View(stud);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProfileManagement( UserTable user)
        {
            var userId = HttpContext.Session.GetInt32("UserId"); //id or text f name and last name 
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            var stud = _dbContext.UserTables.FirstOrDefault(u=> u.UserId == userId.Value);
            if (stud != null)
            {
                stud.FirstName= user.FirstName;
                stud.LastName= user.LastName;
                stud.Email= user.Email;
                stud.PhoneNumber= user.PhoneNumber;
                stud.ProfilePicture= user.ProfilePicture;

                _dbContext.SaveChanges();
            }
            return RedirectToAction("ProfileManagement");
        }
        public IActionResult activities( int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId"); 
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
       
            var borrowBook = _dbContext.Books.Where(b=> b.UserId==userId.Value).Include(b=>b.BookTitle).ToList();
            var room = _dbContext.Rooms.Where(r => r.IsReserved == true && r.UserId == userId.Value).ToList();

            return View(Tuple.Create (borrowBook, room));
        }

        public IActionResult resetPass()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult resetPass(string currentPass , string newPass , string confirmPass)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            var user = _dbContext.UserTables.FirstOrDefault(u => u.UserId == userId.Value);
            if (user==null)
            {
                return RedirectToAction("Login");
            }
            if(newPass != confirmPass)
            {
                ModelState.AddModelError("", "Do not match !!");
                return View();
            }

            user.Password=newPass;
            _dbContext.SaveChanges();
            return View();
        }
        public IActionResult ShowRoomForAdmin() //Admin Dashboard
        {
            var room = _dbContext.Rooms.ToList(); // show all the informaition in the database 
            return View(room);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApproveReservation(int id)
        {
            var room = _dbContext.Rooms.FirstOrDefault(r => r.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            if (room.ReservationStatus == "Pending")
            {
                room.IsReserved = true;
                room.RoomStatus = "Busy";
                _dbContext.SaveChanges();
            }
            return RedirectToAction("ShowRoomForAdmin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id)
        {
            var room = _dbContext.Rooms.FirstOrDefault(r => r.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            if (room.ReservationStatus == "Pending")
            {   
                room.IsReserved = false;
                room.RoomStatus = "Available";
                _dbContext.SaveChanges();
            }
            return RedirectToAction("ShowRoomForAdmin");
        }
        
    }
}


       