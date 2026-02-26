using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class RoomController : Controller
    {
        private readonly MyDbContext _dbContext;

        public RoomController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult ShowRoom()
        {
            var rooms = _dbContext.Rooms
                   .Where(r => r.RoomStatus == "Available")
                   .ToList();
            return View(rooms);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RequestReservation(int id, DateOnly reservationDate, TimeOnly startTime)
        {
            var room = _dbContext.Rooms.FirstOrDefault(r => r.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            // Convert DateOnly + TimeOnly into DateTime
            var requestedDateTime = reservationDate.ToDateTime(startTime);

            if (requestedDateTime < DateTime.Now.AddHours(1))
            {
                TempData["Error"] = "Cannot reserve a room in the past or too soon.";
                return RedirectToAction("ShowRoom");
            }

            if (room.RoomStatus == "Available")
            {
                room.ReservationDate = reservationDate;
                room.StartTime = startTime;
                room.ReservationDuration = 2; // fixed 2 hours
                room.EndTime = startTime.AddHours(2);
                room.ReservationStatus = "Pending";
                room.RoomStatus = "Pending";
                room.IsReserved = true;

                // Associate with logged-in student
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId != null)
                {
                    room.UserId = userId.Value;
                }

                _dbContext.SaveChanges();
            }

            return RedirectToAction("ShowRoom");
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
            if (room.RoomStatus == "Available" && room.ReservationStatus=="Pending")
            {
                room.IsReserved = true;
                room.RoomStatus = "Reserved";
                //room.EndTime = DateOnly.FromDateTime(DateTime.Now.AddHours(2));
                

                _dbContext.SaveChanges();
            }
            return RedirectToAction("ShowRoom");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RejectReservation(int id)
        {
            var room = _dbContext.Rooms.FirstOrDefault(r => r.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            if (room.ReservationStatus == "Pending")
            {
                
                room.RoomStatus = "Available";
                room.IsReserved = false;
                
                _dbContext.SaveChanges();
            }

            return RedirectToAction("ShowRoom");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelReservation(int id)
        {
            var room = _dbContext.Rooms.FirstOrDefault(r => r.RoomId == id);
            if (room == null) return NotFound();

            if (room.ReservationStatus == "Pending")
            {
                room.ReservationStatus = "Available";
                room.RoomStatus = "Available";
                room.IsReserved = false;
                room.ReservationDate = null;
                room.StartTime = default;
                room.EndTime = null;
                room.ReservationDuration = null;
                room.UserId = 0;

                _dbContext.SaveChanges();
            }

            return RedirectToAction("ShowRoom");
        }
        
        
    }
}
