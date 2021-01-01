using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ChatApp.Hubs;
using System.Web.Mvc;
using ChatApp.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ChatApp.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public HomeController() { 
        
        
        }

        public HomeController(ApplicationUserManager userManager) {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [Authorize(Roles ="Admin,User")]
        public ActionResult Index()
        {

            ViewBag.Id = User.Identity.GetUserId();
            return View();
        }


        public ActionResult SentMail() {

            return View();
                
        }

        public JsonResult GetOnlineUser() {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyCon"].ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("Select * from AspNetUsers", connection);
            command.Notification = null;
            SqlDependency dependency = new SqlDependency(command);
            dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

            SqlDataReader reader = command.ExecuteReader();
            var Onlinelist = reader.Cast<IDataRecord>()
                  .Select(x => new {
                      Id = (string)x["Id"],
                      Email = (string)x["Email"],
                      Name = (string)x["UserName"],
                      ImagePath = (string)x["ImagePath"],
                      status = (bool)x["status"],

                  }).ToList();
            connection.Close();
            return Json(new { Onlinelist = Onlinelist }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMessages()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyCon"].ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(" select Messages.Id as 'Id',Messages.[Messages] as 'Messages',Messages.ImagePath as 'ImagePath',a.Id as 'recevier_Id',a.UserName as 'recevier_Name',AspNetUsers.UserName as 'sender_Name',Messages.sender_Id as 'sender_Id',Messages.DateTime as 'datetime' from Messages Inner JOIN AspNetUsers  as a On Messages.recevier_Id = a.Id  Inner JOIN AspNetUsers On Messages.sender_Id = AspNetUsers.Id", connection);
            command.Notification = null;
            SqlDependency dependency = new SqlDependency(command);
            dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

            SqlDataReader reader = command.ExecuteReader();
            var Messagelist = reader.Cast<IDataRecord>()
                  .Select(x => new {
                      Id = (int)x["Id"],
                      Messages = (string)x["Messages"],
                      ImagePath = (string)x["ImagePath"],
                      recevier_Id = (string)x["recevier_Id"],
                      recevier_Name = (string)x["recevier_Name"],
                      sender_Id = (string)x["sender_Id"],
                      sender_Name = (string)x["sender_Name"],
                      datetime =(DateTime)x["datetime"]
                  }).ToList();
            connection.Close();
            return Json(new { Messagelist = Messagelist }, JsonRequestBehavior.AllowGet);
        }

       

        public void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {

            UsersOnline uo = new UsersOnline();
            uo.Check();
            uo.Messagess();
        }

        public JsonResult GetMessageNotification()
        {

            System.Diagnostics.Debug.WriteLine("in home getMessnoti " + Session["LastUpdated"]);
            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponents NC = new NotificationComponents();
            var list = NC.Getmessages(notificationRegisterTime);
            System.Diagnostics.Debug.WriteLine("from noticomoponecnts" + list);
            Session["LastUpdated"] = DateTime.Now;

            System.Diagnostics.Debug.WriteLine("end check sess" + Session["LastUpdated"]);

            return Json(new { list = list }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Create(Message message)
        {

            if (message.ImageFiles != null)
            {
                List<string> list = new List<string>();
                foreach (HttpPostedFileBase postedFileBase in message.ImageFiles)
                {

                    string FileNama = Path.GetFileNameWithoutExtension(postedFileBase.FileName);
                    string extension = Path.GetExtension(postedFileBase.FileName);
                    FileNama = FileNama + DateTime.Now.ToString("yymmssffff") + extension;

                    list.Add("~/Images/" + FileNama);

                    //   string[] str = list.ToArray();


                    FileNama = Path.Combine(Server.MapPath("~/Images/"), FileNama);
                    postedFileBase.SaveAs(FileNama);


                }

                for (var i = 0; i < list.Count(); i++)
                {

                    message.ImagePath += list[i] + ",";
                }
                if (message.Messages == null)
                {
                    message.DateTime = DateTime.Now;
                    message.Messages += "Null";
                    message.sender_Id = User.Identity.GetUserId();
                    db.Messages.Add(message);
                    db.SaveChanges();

                }
                else
                {

                    message.DateTime = DateTime.Now;
                    message.sender_Id = User.Identity.GetUserId();
                    db.Messages.Add(message);
                    db.SaveChanges();
                }
                }
            else
            {
                message.DateTime = DateTime.Now;
                message.sender_Id = User.Identity.GetUserId();
                message.ImagePath += "Null";
                db.Messages.Add(message);
                db.SaveChanges();

            }



        }




        public ActionResult About()
        {
            //ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}