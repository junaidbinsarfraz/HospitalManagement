using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Data.Entity;
using Newtonsoft.Json;

namespace HospitalManagament.Controllers
{
    public class MessageController : Controller
    {
        // GET: Message
        public ActionResult Index()
        {
            if (HttpContext.Session["LoggedInUser"] != null)
            {
                return View((User)HttpContext.Session["LoggedInUser"]);
            }
            return View();
        }

        // Get chat history of patient and doctor
        public ActionResult ChatHistory(int PatientId, int DoctorId)
        {
            HospitalManagementContext db = new HospitalManagementContext();

            List<Message> Messages = db.Messages.Include(u => u.Patient).Include(u => u.Doctor).Where(u => u.DoctorId == DoctorId).Where(u => u.PatientId == PatientId).ToList();

            return Content(JsonConvert.SerializeObject(Messages, Formatting.None,
                            new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            }), "application/json");

        }
    }
}