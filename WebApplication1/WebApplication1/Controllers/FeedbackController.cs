using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FeedbackController : Controller
    {
    private readonly MyDbContext myDbContext;
    public  FeedbackController (MyDbContext _myDbContext)
        {
            myDbContext= _myDbContext;
        }

       
        public IActionResult ShowFeedbackForAdmin()// Admin dashboard
        {
            var feedback = myDbContext.Feedbacks.Include(f => f.User).ToList();
            return View(feedback);
            
        }
        public IActionResult addFeedback()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult addFeedback(Feedback feedback)
        {
            if (ModelState.IsValid!=null )
            {
                feedback.SubmittedDate= DateOnly.FromDateTime(DateTime.Now);
                if( HttpContext.Session.GetInt32("UserId").HasValue)
                { 
                    feedback.UserId= HttpContext.Session.GetInt32("UserId").Value;
                }
                myDbContext.Feedbacks.Add(feedback);
                myDbContext.SaveChanges();
                return RedirectToAction("addFeedback");
            }

            return View(feedback);
        }
        public IActionResult ApproveFeedback(int id) // id -> Fk name 
        {
            var feedback = myDbContext.Feedbacks.FirstOrDefault(f => f.FeedbackId == id);
            if (feedback != null)
            {
                feedback.IsApproved = true;
                myDbContext.SaveChanges();
            }

            return RedirectToAction("ShowFeedbackForAdmin");
        }
        //public IActionResult RejectFeedback(int id) // id -> Fk name 
        //{
        //    var feedback = myDbContext.Feedbacks.FirstOrDefault(f => f.FeedbackId == id);
        //    if (feedback != null)
        //    {
        //        myDbContext.Feedbacks.Remove(feedback);
        //        myDbContext.SaveChanges();
        //    }

        //    return RedirectToAction("ShowFeedbackForAdmin");
        //}

        public IActionResult ShowFeedbackForHome()
        {
            var feedback =myDbContext.Feedbacks.Where(f =>f.IsApproved).Include(f=>f.User).ToList();
            return View(feedback);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
