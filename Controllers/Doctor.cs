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
    public class DoctorController : Controller
    {
        private HospitalManagementContext db = new HospitalManagementContext();

        // GET: Doctor
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
                oldUser.Doctor.Specialization = user.Doctor.Specialization;
                oldUser.Doctor.Designation = user.Doctor.Designation;

                db.SaveChanges();

                HttpContext.Session["LoggedInUser"] = oldUser;

                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Doctor/Edit/5
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
            Doctor doctor = user.Doctor;
            if (doctor == null)
            {
                return HttpNotFound();
            }

            return View(db.Users.FirstOrDefault(a => a.Id == id));
        }
    }
}