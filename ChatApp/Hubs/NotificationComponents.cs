using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ChatApp.Models;

namespace ChatApp.Hubs
{
    public class NotificationComponents
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void RegisterNotification(DateTime currentTime)
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyCon"].ConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(" select Messages.Id as 'Id',Messages.[Messages] as 'Messages',Messages.ImagePath as 'ImagePath',a.Id as 'recevier_Id',a.UserName as 'recevier_Name',AspNetUsers.UserName as 'sender_Name',Messages.sender_Id as 'sender_Id' from Messages Inner JOIN AspNetUsers  as a On Messages.recevier_Id = a.Id  Inner JOIN AspNetUsers On Messages.sender_Id = AspNetUsers.Id where Messages.DateTime > @AddedOn", connection);
            command.Parameters.AddWithValue("@AddedOn", currentTime);
            command.Notification = null;
            SqlDependency dependency = new SqlDependency(command);
            dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

            SqlDataReader reader = command.ExecuteReader();



            System.Diagnostics.Debug.WriteLine("Register Noti working");
            connection.Close();

        }

        public void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("dc in noti working");
            UsersOnline uo = new UsersOnline();
            uo.noti();
            RegisterNotification(DateTime.Now);
        }

        public List<Message> Getmessages(DateTime afterdateTime)
        {

            return db.Messages.Where(x => x.DateTime > afterdateTime).OrderByDescending(y => y.DateTime).ToList();
        }


    }
}