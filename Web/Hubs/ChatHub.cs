using App.Common.Data;
using App.Common.Security.Authentication;
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
            RegisterUserOnline();
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
            DeregisterUserOnline();
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
                SenderID = App.Common.Security.Authentication.User.Current.UserID,
                Date = DateTime.Now
            };
            RepositoryFactory.CreateWithGuid<ChatMessage>().SaveOrUpdate(chatMessage);
        }

        private void RegisterUserOnline()
        {
            ChatHubAvailableUserManager.RegisterUserOnline(User.Current.UserID, User.Current.UserName, Context.ConnectionId);
        }

        private void DeregisterUserOnline()
        {
            ChatHubAvailableUserManager.DeregisterUserOnline(Context.ConnectionId);
        }
        #endregion
    }

    public static class ChatHubAvailableUserManager
    {
        public static List<UserDetail> ConnectedUsers = new List<UserDetail>();

        public static void RegisterUserOnline(Guid userID, string userName, string connectionID)
        {
            if (!ChatHubAvailableUserManager.ConnectedUsers.Any(x => x.UserID == userID && x.ConnectionID == connectionID))
            {
                ChatHubAvailableUserManager.ConnectedUsers.Add(new UserDetail() { UserID = userID, UserName = userName, ConnectionID = connectionID });
            }
        }

        public static void DeregisterUserOnline(string connectionID)
        {
            if (ChatHubAvailableUserManager.ConnectedUsers.Any(x => x.ConnectionID == connectionID))
            {
                var removableUser = ChatHubAvailableUserManager.ConnectedUsers.SingleOrDefault(x => x.ConnectionID == connectionID);
                ChatHubAvailableUserManager.ConnectedUsers.Remove(removableUser);
            }
        }
    }

    public class UserDetail
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string ConnectionID { get; set; }
    }
}