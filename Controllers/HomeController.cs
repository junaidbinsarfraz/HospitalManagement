using HospitalManagament.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HospitalManagament.Controllers
{
    public class HomeController : Controller
    {
        // Show dashboard of admin
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

        // Show login page if user is not loggedin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // Do login for a user and redirect to specific page w.r.t. user role
        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                HospitalManagementContext dataContext = new HospitalManagementContext();
                // Check credentials
                User user = dataContext.Users.FirstOrDefault(u => u.Email == loginModel.Email && u.Password == loginModel.Password);
                
                if (user != null)
                {
                    HttpContext.Session["LoggedInUser"] = user;
                    // Check if admin
                    if (user.Role.Name == "Admin")
                    {
                        HttpContext.Session["Role"] = "Admin";
                        HttpContext.Session["TotalPatientList"] = dataContext.Users.Where(u => u.Patient != null).Where(u => u.Patient.Status == "Admitted").ToList();
                        HttpContext.Session["TotalPatients"] = dataContext.Users.Count(u => u.Patient != null && u.Patient.Status == "Admitted");
                        HttpContext.Session["TotalCaregiverList"] = dataContext.Users.Where(u => u.Caregiver != null).ToList();
                        HttpContext.Session["TotalCareGivers"] = dataContext.Users.Count(u => u.Caregiver != null);
                        HttpContext.Session["TotalDoctorList"] = dataContext.Users.Where(u => u.Doctor != null).ToList();
                        HttpContext.Session["TotalDoctors"] = dataContext.Users.Count(u => u.Doctor != null);

                        return RedirectToAction("Index", "Home");
                    }

                    else if (user.Role.Name == "Patient")
                    {
                        HttpContext.Session["Patient"] = user.Patient;
                        HttpContext.Session["PatientId"] = user.Patient.Id;
                        HttpContext.Session["Doctor"] = null;
                        HttpContext.Session["DoctorId"] = -1;
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
                        HttpContext.Session["Patient"] = null;
                        HttpContext.Session["PatientId"] = -1;
                        HttpContext.Session["Doctor"] = user.Doctor;
                        HttpContext.Session["DoctorId"] = user.Doctor.Id;
                        HttpContext.Session["Role"] = "Doctor";
                        return RedirectToAction("Index", "Doctor");
                    }
                }
                // Invalid credentials
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }

            return View();
        }

        // Do logout
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
        
        // Fetch data for Filled line chart
        [HttpGet]
        public ActionResult CountGenderPerMonthFilledLine()
        {
            HospitalManagementContext dataContext = new HospitalManagementContext();

            List<CountGendersPerMonth> GenderMonth = dataContext.Database.SqlQuery<CountGendersPerMonth>("Select u.Gender, p.EntryDate ActualDate, datename(month, DATEPART(MONTH, p.EntryDate)) month, DATEPART(MONTH, p.EntryDate) monthnumber, COUNT(p.User_Id) count from [HospitalManagement].[dbo].[Patients] p,[HospitalManagement].[dbo].[Users] u where u.Id = p.User_Id and p.Status = 'Admitted' group by DATEPART(MONTH, p.EntryDate), p.EntryDate, u.Gender").ToList();

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
        }

        // Fetch data for hollow line chart
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

            List<CountGendersPerMonth> GenderMonth = dataContext.Database.SqlQuery<CountGendersPerMonth>("select datename(month, p.EntryDate) Month, count(case when u.Gender = 'Female' then 1 end) FemaleCount, count(case when u.Gender = 'Male' then 1 end) MaleCount from [HospitalManagement].[dbo].[Patients] p left join [HospitalManagement].[dbo].[Users] u on p.User_Id = u.Id where p.Status = 'Admitted' group by datename(month, p.EntryDate);").ToList();

            for (int i = 0; i < GenderMonthFinal.Count; i++)
            {
                for (int j = 0; j < GenderMonth.Count; j++)
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

        // Fetch data for pie chart to show atmost top 5 patients w.r.t disease percentage
        public ActionResult PatientDiseasePercentage()
        {
            HospitalManagementContext DataContext = new HospitalManagementContext();

            List<DiseasePercentage> DiseasePercentages = DataContext.Database.SqlQuery<DiseasePercentage>("select top 5 count(p.Disease) Count, p.Disease Label from[HospitalManagement].[dbo].Patients p where p.Status = 'Admitted' group by p.Disease order by count(p.Disease) desc").ToList();

            // Set color of each percecnage section

            int TotalPatients = DataContext.Patients.Where(p => p.Status == "Admitted").ToList().Count;

            if (DiseasePercentages.Count > 0)
            {
                DiseasePercentages[0].Value = Math.Round(((double)(DiseasePercentages[0].Count) / TotalPatients) * 100, 2);
                DiseasePercentages[0].Color = "#13dafe";
                DiseasePercentages[0].Highlight = "#13dafe";
            }
            if (DiseasePercentages.Count > 1)
            {
                DiseasePercentages[1].Value = Math.Round(((double)(DiseasePercentages[1].Count) / TotalPatients) * 100, 2);
                DiseasePercentages[1].Color = "#6164c1";
                DiseasePercentages[1].Highlight = "#6164c1";
            }
            if (DiseasePercentages.Count > 2)
            {
                DiseasePercentages[2].Value = Math.Round(((double)(DiseasePercentages[2].Count) / TotalPatients) * 100, 2);
                DiseasePercentages[2].Color = "#99d683";
                DiseasePercentages[2].Highlight = "#99d683";
            }

            if (DiseasePercentages.Count > 3)
            {
                DiseasePercentages[3].Value = Math.Round(((double)(DiseasePercentages[3].Count) / TotalPatients) * 100, 2);
                DiseasePercentages[3].Color = "#ffca4a";
                DiseasePercentages[3].Highlight = "#ffca4a";
            }
            if (DiseasePercentages.Count > 4)
            {
                DiseasePercentages[4].Value = Math.Round(((double)(DiseasePercentages[4].Count) / TotalPatients) * 100, 2);
                DiseasePercentages[4].Color = "#4c5667";
                DiseasePercentages[4].Highlight = "#4c5667";
            }

            return Content(JsonConvert.SerializeObject(DiseasePercentages), "application/json");

        }

        // Get updated count list of patients, caregiver and doctor
        [HttpGet]
        public ActionResult GetUpdatedCountsAndList()
        {
            User user = (User) HttpContext.Session["LoggedInUser"];

            if (user !=null && user.Role.Name == "Admin")
            {
                HospitalManagementContext db = new HospitalManagementContext();

                HttpContext.Session["TotalPatientList"] = db.Users.Include(u => u.Patient).Where(u => u.Patient != null).Where(u => u.Patient.Status == "Admitted").ToList();
                HttpContext.Session["TotalPatients"] = db.Users.Count(u => u.Patient != null && u.Patient.Status == "Admitted");
                HttpContext.Session["TotalCaregiverList"] = db.Users.Include(u => u.Caregiver).Where(u => u.Caregiver != null).ToList();
                HttpContext.Session["TotalCareGivers"] = db.Users.Count(u => u.Caregiver != null);
                HttpContext.Session["TotalDoctorList"] = db.Users.Include(u => u.Doctor).Where(u => u.Doctor != null).ToList();
                HttpContext.Session["TotalDoctors"] = db.Users.Count(u => u.Doctor != null);
            }

            return Json(new
            {
                Status = "updated"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}