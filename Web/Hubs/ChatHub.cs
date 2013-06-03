using App.Common.Data;
using App.Domain.Models.Chat;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Hubs
{
    public class ChatHub : Hub
    {

        private IRepositoryFactory repositoryFactory;
        protected IRepositoryFactory RepositoryFactory
        {
            get
            {
                if(repositoryFactory == null)
                    repositoryFactory = ServiceLocator.Current.GetInstance<IRepositoryFactory>();
                return repositoryFactory;
            }
        }

        public void Send(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            var clientUserName = App.Common.Security.Authentication.User.Current.UserName;
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(clientUserName, message);
            StoreChatMessage(message);
        }

        private void StoreChatMessage(string message)
        {
            var chatMessage = new ChatMessage()
            {
                Message = message,
                UserName = App.Common.Security.Authentication.User.Current.UserName,
                UserProfileID = App.Common.Security.Authentication.User.Current.UserID,
                Date = DateTime.Now
            };
            var profiles = RepositoryFactory.CreateWithGuid<ChatMessage>().SaveOrUpdate(chatMessage);
        }
    }
}