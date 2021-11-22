using chat_application.Domain.Models;
using System;
using System.Collections.Generic;

namespace chat_application.Infra.Data.Interface
{
    public interface IChatRepository
    {
        User Add(string ConnectionId, User user);
        void Disconnect(string ConnectionId);
        IList<User> GetUsers();
        User GetUserByKey(Int64 key);
    }
}
