using Cassandra.Data.Linq;
using ChatHubSolution.Data.Entities;
using ChatHubSolution.Implementation.Interfaces;
using ChatHubSolution.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatHubSolution.Hubs
{
    public class ChatHub : Hub
    {
        private readonly Cassandra.ISession _session;

        public ChatHub(ICassandraProvider provider)
        {
            _session = provider.GetSession();
        }

        public async Task JoinChat(UserConnection conn)
        {
            await Clients.All.SendAsync("ReceiveMessage", conn.SenderName, $"{conn.SenderName} has joined conversation {conn.ConversationId}");
        }

        public async Task JoinSpecificChatRoom(UserConnection conn)
        {
            var conversations = new Table<Conversation>(_session);
            var conversation = await conversations.FirstOrDefault(c => new List<string> { conn.SenderId, conn.ReceiverId }.Equals(new List<string> { c.UserOneId, c.UserTwoId })).ExecuteAsync();
            if (conversation == null)
            {
                var newConversation = new Conversation
                {
                    Id = Context.ConnectionId,
                    UserOneId = conn.SenderId,
                    UserTwoId = conn.ReceiverId,
                    Status = "ACTIVE",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                await conversations.Insert(newConversation).ExecuteAsync();

                await Groups.AddToGroupAsync(Context.ConnectionId, newConversation.Id);

                await Clients.Group(newConversation.Id).SendAsync("ReceiveMessage", conn.SenderId, $"{conn.SenderName} has created  conversation {newConversation.Id}");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, conversation.Id);
            await Clients.Group(conversation.Id).SendAsync("ReceiveMessage", conn.SenderId, $"{conn.SenderName} has joined  conversation {conversation.Id}");
        }

        public async Task SendMessage(string senderId, string msg)
        {
            var conversations = new Table<Conversation>(_session);
            var conversation = await conversations.FirstOrDefault(c => c.ConnectionId.Equals(Context.ConnectionId)).ExecuteAsync();

            if (conversation != null)
            {
                await Clients.Group(conversation.Id).SendAsync("ReceiveSpecificMessage", senderId, msg);

                var messages = new Table<Message>(_session);
                await messages.Insert(new Message
                {
                    Id = conversation.Id,
                    Content = msg,
                    ConversationId = conversation.Id,
                    Status = "ACTIVE",
                    UserId = senderId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ExecuteAsync();
            }
        }
    }
}
