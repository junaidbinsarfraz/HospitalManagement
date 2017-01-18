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

                // add role as patient 
                user.Role = db.Roles.ToList().Where(u => u.Name == "Patient").FirstOrDefault();

                db.Users.Add(user);

                db.SaveChanges();

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

                // temp sol
                //oldUser.FullName = user.FullName;
                //oldUser.UserName = user.UserName;
                //oldUser.Password = user.Password;
                //oldUser.Patient.NRIC = user.Patient.NRIC;
                //oldUser.Patient.Age = user.Patient.Age;
                //oldUser.Patient.ContactNo = user.Patient.ContactNo;
                //oldUser.Patient.Disease = user.Patient.Disease;
                //oldUser.Email = user.Email;
                //oldUser.Patient.Occupation = user.Patient.Occupation;
                //oldUser.Patient.Gender = user.Patient.Gender;
                //oldUser.Patient.Address = user.Patient.Address;

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

                db.SaveChanges();

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

            //UsersRole userRole = db.UsersRoles.ToList().Where(u => u.UsreId == id).First();
            //if (userRole == null)
            //{
            //    return HttpNotFound();
            //}
            //db.UsersRoles.Remove(userRole);

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
