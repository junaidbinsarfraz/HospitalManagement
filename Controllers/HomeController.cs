using HospitalManagament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospitalManagament.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (HttpContext.Session["LoggedInUser"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            else
            {
                return View();
            }

        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                HospitalManagementContext dataContext = new HospitalManagementContext();
                User user = dataContext.Users.FirstOrDefault(u => u.Email == loginModel.Email && u.Password == loginModel.Password);
                if (user != null)
                {
                    HttpContext.Session["LoggedInUser"] = user;
                    if (user.Role.Name == "Admin")
                    {
                        HttpContext.Session["Role"] = "Admin";
                        HttpContext.Session["TotalPatientList"] = dataContext.Users.Where(u => u.Patient != null).ToList();
                        HttpContext.Session["TotalPatients"] = dataContext.Users.Count(u => u.Patient != null);
                        HttpContext.Session["TotalCaregiverList"] = dataContext.Users.Where(u => u.Caregiver != null).ToList();
                        HttpContext.Session["TotalCareGivers"] = dataContext.Users.Count(u => u.Caregiver != null);
                        HttpContext.Session["TotalDoctorList"] = dataContext.Users.Where(u => u.Doctor != null).ToList();
                        HttpContext.Session["TotalDoctors"] = dataContext.Users.Count(u => u.Doctor != null);

                        // Chart - JSON
                        //dataContext.Users.Include(a => a.)

                        var months = dataContext.Patients
                        .AsEnumerable()
                        .Select(c => new
                        {
                            MonthName = c.EntryDate.Value.ToString("MMMM")
                        }).ToList();

                        var GenderMonth = dataContext.Database.SqlQuery<CountGendersPerMonth>("Select u.Gender, datename(month, DATEPART(MONTH, p.EntryDate)) month, DATEPART(MONTH, p.EntryDate) monthnumber, COUNT(p.User_Id) count from [HospitalManagement].[dbo].[Patients] p,[HospitalManagement].[dbo].[Users] u where u.Id = p.User_Id group by DATEPART(MONTH, p.EntryDate), u.Gender").ToList();

                        // Create months list
                        var labels = new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

                        var MaleCount = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                        var FemaleCount = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };



                        return RedirectToAction("Index", "Home");
                    }

                    else if (user.Role.Name == "Patient")
                    {
                        HttpContext.Session["Role"] = "Patient";
                        return RedirectToAction("Index", "Patient");
                    }

                    else if (user.Role.Name == "Caregiver")
                    {
                        HttpContext.Session["Role"] = "Caregiver";
                        return RedirectToAction("Index", "CareGiver");
                    }

                    else if (user.Role.Name == "Doctor")
                    {
                        HttpContext.Session["Role"] = "Doctor";
                        return RedirectToAction("Index", "Doctor");
                    }
                }

                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            HttpContext.Session["LoggedInUser"] = null;
            HttpContext.Session["Role"] = null;
            HttpContext.Session["TotalCareGivers"] = null;
            HttpContext.Session["TotalPatients"] = null;
            HttpContext.Session["TotalDoctors"] = null;
            HttpContext.Session["TotalPatientList"] = null;
            HttpContext.Session["TotalDoctorList"] = null;
            HttpContext.Session["TotalCaregiverList"] = null;

            return RedirectToAction("Login", "Home");
        }
    }
}