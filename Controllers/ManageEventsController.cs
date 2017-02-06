using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HospitalManagament.Controllers
{
    public class ManageEventsController : Controller
    {
        private HospitalManagementContext db = new HospitalManagementContext();

        // GET: ManageEvents
        public ActionResult Index()
        {
            // Get logged in user
            User user = (User)HttpContext.Session["LoggedInUser"];

            if(user != null && (user.Role.Name == "Admin" || user.Role.Name == "Doctor"))
            {
                var events = db.Events.ToList().Where(e => e.UserId == user.Id);
                return View(events);
            }

            return HttpNotFound();            
        }

        // GET: ManageEvents/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event event1 = db.Events.Find(id);
            if (event1 == null)
            {
                return HttpNotFound();
            }
            return View(event1);
        }

        // GET: ManageEvents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Event event1)
        {
            if (ModelState.IsValid)
            {
                // Add date and time
                var StartDate = DateTime.ParseExact(event1.StartDateStr, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                event1.Start = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, event1.StartTime.Value.Hours, event1.StartTime.Value.Minutes, event1.StartTime.Value.Seconds);

                var EndDate = DateTime.ParseExact(event1.EndDateStr, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                event1.End = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, event1.EndTime.Value.Hours, event1.EndTime.Value.Minutes, event1.EndTime.Value.Seconds);

                // Create url

                User user = (User)HttpContext.Session["LoggedInUser"];

                event1.User = db.Users.ToList().Where(u => u.Id == user.Id).FirstOrDefault();

                db.Events.Add(event1);

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(event1);
        }

        // GET: ManageEvents/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event event1 = db.Events.Find(id);
            if (event1 == null)
            {
                return HttpNotFound();
            }
            return View(event1);
        }

        // POST: ManageEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(Event event1)
        {
            if (ModelState.IsValid)
            {
                // Get logged in user
                User user = (User)HttpContext.Session["LoggedInUser"];

                Event oldEvent = db.Events.FirstOrDefault(e => e.Id == event1.Id);

                var StartDate = DateTime.ParseExact(event1.StartDateStr, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                event1.Start = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, event1.StartTime.Value.Hours, event1.StartTime.Value.Minutes, event1.StartTime.Value.Seconds);

                var EndDate = DateTime.ParseExact(event1.EndDateStr, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                event1.End = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, event1.EndTime.Value.Hours, event1.EndTime.Value.Minutes, event1.EndTime.Value.Seconds);

                oldEvent.Name = event1.Name;
                oldEvent.Description = event1.Description;
                oldEvent.allDay = event1.allDay;
                oldEvent.End = event1.End;
                oldEvent.EndDateStr = event1.EndDateStr;
                oldEvent.EndTime = event1.EndTime;
                oldEvent.Start = event1.Start;
                oldEvent.StartDateStr = event1.StartDateStr;
                oldEvent.StartTime = event1.StartTime;
                oldEvent.Status = event1.Status;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(event1);
        }

        // GET: ManageEvents/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Event event1 = db.Events.Find(id);
            if (event1 == null)
            {
                return HttpNotFound();
            }

            db.Events.Remove(event1);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: ManageEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Event event1 = db.Events.Find(id);
            if (event1 == null)
            {
                return HttpNotFound();
            }

            db.Events.Remove(event1);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Fetch All event for loggedin user
        [HttpGet]
        public ActionResult AllEvents()
        {
            User user = (User)HttpContext.Session["LoggedInUser"];

            List<Event> allEvents = new List<Event>();

            if (user != null)
            {
                HospitalManagementContext db = new HospitalManagementContext();

                allEvents = db.Database.SqlQuery<Event>("select * from[HospitalManagement].[dbo].Events e where e.UserId = " + user.Id).ToList();

                //allEvents = (List<Event>) db.Events.ToList().Where(e => e.UserId == user.Id);
            }

            return Content(JsonConvert.SerializeObject(allEvents), "application/json");
        }
    }
}
