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

        //// GET: ManageCareGivers
        //public ActionResult Index()
        //{
        //    return View(db.Caregivers.ToList());
        //}

        // GET: ManageCareGivers
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Caregiver);
            return View(users.ToList().Where(x => x.UserName != "Admin").Where(x => x.Caregiver != null));
            //return View(db.Caregivers.Include(u => u.Users).ToList());
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
                caregiver.Patient = db.Patients.ToList().Where(u => u.Id == user.Id).FirstOrDefault();

                // add role as caregiver 
                user.Role = db.Roles.ToList().Where(u => u.Name == "Caregiver").FirstOrDefault();
                user.Caregiver = caregiver;

                db.Users.Add(user);

                db.SaveChanges();

                User newUser = db.Users.ToList().Last();

                return RedirectToAction("Index");
            }

            ViewBag.PatientId = new SelectList(db.Patients.Include(a => a.User).Select(a => new { a.User.FullName, a.Id }), "Id", "FullName", user.Patient.Id);
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
            ViewBag.PatientId = new SelectList(db.Patients.Include(a => a.User).Select(a => new { a.User.FullName, a.Id }), "Id", "FullName", careGiver.Patient.Id);
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
                //Caregiver oldCareGiver = db.Caregivers.FirstOrDefault(c => c.Id == user.CareGiverId);
                //oldCareGiver.NRIC = user.Caregiver.NRIC;
                //oldCareGiver.Age = user.Caregiver.Age;
                //oldCareGiver.ContactNo = user.Caregiver.ContactNo;
                //oldCareGiver.Gender = user.Caregiver.Gender;
                //oldCareGiver.Address = user.Caregiver.Address;
                //oldCareGiver.PatientId = user.Caregiver.PatientId;

                //User oldUser = oldCareGiver.Users.FirstOrDefault();
                //oldUser.Email = user.Email;
                //oldUser.UserName = user.UserName;
                //oldUser.FullName = user.FullName;
                //oldUser.Password = user.Password;

                User oldUser = db.Users.FirstOrDefault(u => u.Id == user.Id);

                oldUser.FullName = user.FullName;
                oldUser.UserName = user.UserName;
                oldUser.NRIC = user.NRIC;
                oldUser.Age = user.Age;
                oldUser.ContactNo = user.ContactNo;
                oldUser.Email = user.Email;
                oldUser.Gender = user.Gender;
                oldUser.Address = user.Address;
                oldUser.Caregiver.Patient = db.Patients.ToList().Where(u => u.Id == user.Patient.Id).FirstOrDefault();

                db.SaveChanges();

                return RedirectToAction("Index");
            }

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

            //UsersRole userRole = db.UsersRoles.ToList().Where(u => u.UsreId == id).First();
            //if (userRole == null)
            //{
            //    return HttpNotFound();
            //}
            //db.UsersRoles.Remove(userRole);

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
