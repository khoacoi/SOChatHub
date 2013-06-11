using App.Common.Data;
using App.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.ViewModels.Chat;
using App.Common.Extensions;

namespace Web.Controllers.Chat.Queries
{
    public class ChatViewModelQuery : QueryBase, IChatViewModelQuery
    {
        public ViewModels.Chat.ChatViewModel GetChatViewModel()
        {
            ChatViewModel viewModel = new ChatViewModel();
            // Get all user profile except current one.
            var availabelUsers = this.Session.QueryOver<UserProfile>().Where(x => x.Id != App.Common.Security.Authentication.User.Current.UserID).List();
            if (availabelUsers!= null && availabelUsers.Any())
            {
                availabelUsers.Each(x =>
                {
                    viewModel.AvailableUser.Add(new UserProfileViewModel() { UserID = x.Id, UserName = x.UserName });
                });
            }

            return viewModel;
        }
    }
}