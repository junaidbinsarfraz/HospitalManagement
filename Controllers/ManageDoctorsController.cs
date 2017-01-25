using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HospitalManagament.Controllers
{
    public class ManageDoctorsController : Controller
    {
        private HospitalManagementContext db = new HospitalManagementContext();

        // GET: ManageDoctors
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Doctor);
            return View(users.ToList().Where(x => x.UserName != "Admin").Where(x => x.Doctor != null));
        }

        // GET: ManageDoctors/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: ManageDoctors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageDoctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                // add role as doctor
                user.Role = db.Roles.ToList().Where(u => u.Name == "Doctor").FirstOrDefault();

                db.Users.Add(user);

                db.SaveChanges();

                User Admin = (User)HttpContext.Session["LoggedInUser"];

                // Update totals count
                if (Admin != null && Admin.Role.Name == "Admin")
                {
                    HttpContext.Session["TotalPatientList"] = db.Users.Include(u => u.Patient).Where(u => u.Patient != null).Where(u => u.Patient.Status == "Admitted").ToList();
                    HttpContext.Session["TotalPatients"] = db.Users.Count(u => u.Patient != null && u.Patient.Status == "Admitted");
                    HttpContext.Session["TotalCaregiverList"] = db.Users.Include(u => u.Caregiver).Where(u => u.Caregiver != null).ToList();
                    HttpContext.Session["TotalCareGivers"] = db.Users.Count(u => u.Caregiver != null);
                    HttpContext.Session["TotalDoctorList"] = db.Users.Include(u => u.Doctor).Where(u => u.Doctor != null).ToList();
                    HttpContext.Session["TotalDoctors"] = db.Users.Count(u => u.Doctor != null);
                }

                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: ManageDoctors/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: ManageDoctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                User oldUser = db.Users.FirstOrDefault(u => u.Id == user.Id);

                oldUser.FullName = user.FullName;
                oldUser.UserName = user.UserName;
                oldUser.NRIC = user.NRIC;
                oldUser.Age = user.Age;
                oldUser.ContactNo = user.ContactNo;
                oldUser.Email = user.Email;
                oldUser.Gender = user.Gender;
                oldUser.Address = user.Address;
                oldUser.Comments = user.Comments;
                oldUser.Doctor.Designation = user.Doctor.Designation;
                oldUser.Doctor.Specialization = user.Doctor.Specialization;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: ManageDoctors/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            db.Doctors.Remove(user.Doctor);

            db.Users.Remove(user);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: ManageDoctors/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
    }
}
