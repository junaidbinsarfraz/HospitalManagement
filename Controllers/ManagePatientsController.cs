using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace HospitalManagament.Controllers
{
    public class ManagePatientsController : Controller
    {
        private HospitalManagementContext db = new HospitalManagementContext();

        // GET: ManagePatients
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Caregiver).Include(u => u.Patient);
            return View(users.ToList().Where(x => x.UserName != "Admin").Where(x => x.Patient != null));
        }

        // GET: ManagePatients/Details/5
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

        // GET: ManagePatients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManagePatients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {

                user.Patient.EntryDate = DateTime.ParseExact(user.Patient.EntryDateStr, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                // add role as patient 
                user.Role = db.Roles.ToList().Where(u => u.Name == "Patient").FirstOrDefault();
                user.Patient.Status = "Admitted";

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

        // GET: ManagePatients/Edit/5
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

        // POST: ManagePatients/Edit/5
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
                oldUser.Patient.Disease = user.Patient.Disease;
                oldUser.Email = user.Email;
                oldUser.Patient.Occupation = user.Patient.Occupation;
                oldUser.Gender = user.Gender;
                oldUser.Address = user.Address;
                oldUser.Comments = user.Comments;
                oldUser.Patient.Status = user.Patient.Status;

                db.SaveChanges();

                HttpContext.Session["TotalPatientList"] = db.Users.Include(u => u.Patient).Where(u => u.Patient != null).Where(u => u.Patient.Status == "Admitted").ToList();
                HttpContext.Session["TotalPatients"] = db.Users.Count(u => u.Patient != null && u.Patient.Status == "Admitted");

                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: ManagePatients/Delete/5
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
            try
            {
                if (user.Patient.Caregiver != null)
                {
                    Caregiver caregiver = db.Caregivers.ToList().Where(u => u.Patient.Id == user.Patient.Id).First();
                    caregiver.Patient = null;
                }
            }
            catch (Exception ex)
            { }

            db.Patients.Remove(user.Patient);

            db.Users.Remove(user);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: ManagePatients/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("Checkout")]
        // GET: ManagePatients/Delete/5
        public ActionResult Checkout(long? id)
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

            user.Patient.Status = "Not Admitted";

            db.SaveChanges();

            HttpContext.Session["TotalPatientList"] = db.Users.Include(u => u.Patient).Where(u => u.Patient != null).Where(u => u.Patient.Status == "Admitted").ToList();
            HttpContext.Session["TotalPatients"] = db.Users.Count(u => u.Patient != null && u.Patient.Status == "Admitted");

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
