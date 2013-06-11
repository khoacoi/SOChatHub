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

        public void SendToAll(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            var clientUserName = App.Common.Security.Authentication.User.Current.UserName;
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(clientUserName, message);
            StoreChatMessage(message);
        }

        public void SendPrivateMessage(string toUserID, string message)
        {
        }

        #region override functions
        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
            return base.OnDisconnected();
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            return base.OnReconnected();
        }
        #endregion

        #region private functions
        private void StoreChatMessage(string message)
        {
            var chatMessage = new ChatMessage()
            {
                Message = message,
                UserName = App.Common.Security.Authentication.User.Current.UserName,
                UserProfileID = App.Common.Security.Authentication.User.Current.UserID,
                Date = DateTime.Now
            };
            RepositoryFactory.CreateWithGuid<ChatMessage>().SaveOrUpdate(chatMessage);
        }
        #endregion
    }
}