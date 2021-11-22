using chat_application.Domain.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Text.Json;
using System;
using chat_application.Infra.Data.Interface;

namespace chat_application.Application.HubBase
{
    public class ChatHub : Hub
    {
        private readonly IChatRepository _repository;
        private readonly Int64 publicId = 12345678910;

        public ChatHub(IChatRepository chatRepository) => _repository = chatRepository;

        public override Task OnConnectedAsync()
        {
            var userQuery = JsonSerializer.Deserialize<User>(Context.GetHttpContext().Request.Query["user"]);
            var user = _repository.Add(Context.ConnectionId, userQuery);
            Clients.All.SendAsync("chat", _repository.GetUsers(), user);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            _repository.Disconnect(Context.ConnectionId);
            Clients.All.SendAsync("chat", _repository.GetUsers());

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(ChatMessage chat)
        {
            if (chat.toId.Equals(publicId))
            {
                await Clients.All.SendAsync("Public", chat.from, chat.message);
                return;
            }
            var connection = _repository.GetUserByKey(chat.toId).connectionHost;
            await Clients.Client(connection).SendAsync("Receive", chat.from, chat.message);
        }
    }
}
