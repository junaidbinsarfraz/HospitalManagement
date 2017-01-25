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
    public class ManageCareGiversController : Controller
    {
        private HospitalManagementContext db = new HospitalManagementContext();

        // GET: ManageCareGivers
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Caregiver);
            return View(users.ToList().Where(x => x.UserName != "Admin").Where(x => x.Caregiver != null));
        }

        // GET: ManageCareGivers/Details/5
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

        // GET: ManageCareGivers/Create
        public ActionResult Create()
        {
            User user = new User();
            user.Patient = new Patient();

            ViewBag.PatientId = new SelectList(db.Patients.Include(a => a.User).Select(a => new { a.User.FullName, a.Id }), "Id", "FullName");
            return View(user);
        }

        // POST: ManageCareGivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                // add careGiver
                Caregiver caregiver = new Caregiver();
                
                // add role as caregiver 
                user.Role = db.Roles.ToList().Where(u => u.Name == "Caregiver").FirstOrDefault();
                user.Caregiver = caregiver;

                db.Users.Add(user);

                db.SaveChanges();

                User newUser = db.Users.ToList().Last();

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

            ViewBag.PatientId = new SelectList(db.Patients.Include(a => a.User).Select(a => new { a.User.FullName, a.Id }), "Id", "FullName");
            return View(user);
        }

        // GET: ManageCareGivers/Edit/5
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
            Caregiver careGiver = user.Caregiver;
            if (careGiver == null)
            {
                return HttpNotFound();
            }
            // Fetch all patients
            ViewBag.PatientId = new SelectList(db.Patients.Include(a => a.User).Select(a => new { a.User.FullName, a.Id }), "Id", "FullName", 2);
            return View(db.Users.FirstOrDefault(a => a.Id == id));
        }

        // POST: ManageCareGivers/Edit/5
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
                
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine(error);
                        if(error.ErrorMessage == "The Id field is required.")
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
                            
                            db.SaveChanges();

                            return RedirectToAction("Index");
                        }
                    }
                }

                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                System.Diagnostics.Debug.WriteLine(errors);
            }

            ViewBag.PatientId = new SelectList(db.Patients.Include(a => a.User).Select(a => new { a.User.FullName, a.Id }), "Id", "FullName", 2);
            return View(user);
        }

        // GET: ManageCareGivers/Delete/5
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
            return View(user);
        }

        // POST: ManageCareGivers/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
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

            user.Caregiver.Patient = null;

            db.Caregivers.Remove(user.Caregiver);

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
