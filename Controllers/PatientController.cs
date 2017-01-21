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
    public class PatientController : Controller
    {
        private HospitalManagementContext db = new HospitalManagementContext();

        // GET: Patient
        public ActionResult Index()
        {
            if (HttpContext.Session["LoggedInUser"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            else
            {
                return View(HttpContext.Session["LoggedInUser"] as User);
            }
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
                oldUser.Comments = user.Comments;

                db.SaveChanges();

                HttpContext.Session["LoggedInUser"] = oldUser;

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
            //ViewBag.CareGiverId = new SelectList(db.Caregivers, "Id", "ContactNo", user.CareGiverId);
            //ViewBag.PatientId = new SelectList(db.Patients, "Id", "ContactNo", user.PatientId);
            return View(user);
        }
    }
}