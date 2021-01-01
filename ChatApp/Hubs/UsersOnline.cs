using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;


namespace ChatApp.Hubs
{
    [HubName("usersOnline")]
    public class UsersOnline : Hub
    {

        [HubMethodName("announce")]

        public void Announce(string message)
        {
            Clients.All.Announce(message);
           
        }



        [HubMethodName("checkOnline")]
        public void Check() {
    

            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<UsersOnline>();
            context.Clients.All.CheckOnline();
        
        }





        [HubMethodName("messages")]
        public void Messagess()
        {


            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<UsersOnline>();
            context.Clients.All.messages();

        }

        [HubMethodName("notify")]
        public void noti() {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<UsersOnline>();
            context.Clients.All.notify("added");
           
        }


    }
}