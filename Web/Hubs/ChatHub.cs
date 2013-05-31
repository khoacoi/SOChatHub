using App.Domain.Models.User;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string message)
        {
            var clientUserName = App.Common.Security.Authentication.User.Current.UserName;
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(clientUserName, message);
        }
    }
}