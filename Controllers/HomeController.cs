using HospitalManagament.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

        [HttpGet]
        public ActionResult CountGenderPerMonthFilledLine()
        {
            HospitalManagementContext dataContext = new HospitalManagementContext();

            List<CountGendersPerMonth> GenderMonth = dataContext.Database.SqlQuery<CountGendersPerMonth>("Select u.Gender, p.EntryDate ActualDate, datename(month, DATEPART(MONTH, p.EntryDate)) month, DATEPART(MONTH, p.EntryDate) monthnumber, COUNT(p.User_Id) count from [HospitalManagement].[dbo].[Patients] p,[HospitalManagement].[dbo].[Users] u where u.Id = p.User_Id group by DATEPART(MONTH, p.EntryDate), p.EntryDate, u.Gender").ToList();

            // Create months list
            var labels = new List<string>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            var MaleCount = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var FemaleCount = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < GenderMonth.Count; i++)
            {
                if (GenderMonth[i].Gender == "Male")
                {
                    MaleCount[GenderMonth[i].MonthNumber - 1] = GenderMonth[i].Count;
                }
                else
                {
                    FemaleCount[GenderMonth[i].MonthNumber - 1] = GenderMonth[i].Count;
                }
            }

            return Json(new
            {
                labels = labels,
                MaleCount = MaleCount,
                FemaleCount = FemaleCount
            }, JsonRequestBehavior.AllowGet);

            //return Json(GenderMonth, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CountGenderPerMonthHollow()
        {
            HospitalManagementContext dataContext = new HospitalManagementContext();

            List<CountGendersPerMonth> GenderMonthFinal = new List<CountGendersPerMonth>()
            {
                new CountGendersPerMonth(0 , 0, "January"),
                new CountGendersPerMonth(0 , 0, "February"),
                new CountGendersPerMonth(0 , 0, "March"),
                new CountGendersPerMonth(0 , 0, "April"),
                new CountGendersPerMonth(0 , 0, "May"),
                new CountGendersPerMonth(0 , 0, "June"),
                new CountGendersPerMonth(0 , 0, "July"),
                new CountGendersPerMonth(0 , 0, "August"),
                new CountGendersPerMonth(0 , 0, "September"),
                new CountGendersPerMonth(0 , 0, "October"),
                new CountGendersPerMonth(0 , 0, "November"),
                new CountGendersPerMonth(0 , 0, "December")
            };

            List<CountGendersPerMonth> GenderMonth = dataContext.Database.SqlQuery<CountGendersPerMonth>("select datename(month, p.EntryDate) Month, count(case when u.Gender = 'Female' then 1 end) FemaleCount, count(case when u.Gender = 'Male' then 1 end) MaleCount from [HospitalManagement].[dbo].[Patients] p left join [HospitalManagement].[dbo].[Users] u on p.User_Id = u.Id group by datename(month, p.EntryDate); ").ToList();

            for (int i = 0; i < GenderMonthFinal.Count; i++)
            {
                for (int j = 0; j < GenderMonth.Count; j++ )
                {
                    if (GenderMonth[j].Month == GenderMonthFinal[i].Month)
                    {
                        GenderMonthFinal[i].MaleCount = GenderMonth[j].MaleCount;
                        GenderMonthFinal[i].FemaleCount = GenderMonth[j].FemaleCount;
                    }
                }
            }

            return Content(JsonConvert.SerializeObject(GenderMonthFinal), "application/json");
        }
    }
}