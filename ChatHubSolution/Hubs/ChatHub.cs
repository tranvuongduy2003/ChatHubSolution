using Cassandra.Data.Linq;
using ChatHubSolution.Data.Entities;
using ChatHubSolution.DTOs;
using ChatHubSolution.Implementation.Interfaces;
using ChatHubSolution.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

namespace ChatHubSolution.Hubs
{
    public class ChatHub : Hub
    {
        private readonly Cassandra.ISession _session;

        public ChatHub(ICassandraProvider provider)
        {
            _session = provider.GetSession();
        }

        public async Task JoinSpecificChatRoom(UserConnection conn)
        {
            if (conn.ReceiverId.IsNullOrEmpty())
                throw new Exception("ReceiverId is required");
            if (conn.SenderId.IsNullOrEmpty())
                throw new Exception("SenderId is required");

            var getConversation = await _session.PrepareAsync("SELECT * FROM conversations WHERE useroneid = ? AND usertwoid = ? ALLOW FILTERING");
            var row1 = (await _session.ExecuteAsync(getConversation.Bind(conn.SenderId, conn.ReceiverId))).FirstOrDefault();
            var row2 = (await _session.ExecuteAsync(getConversation.Bind(conn.ReceiverId, conn.SenderId))).FirstOrDefault();

            if (row1 == null && row2 == null)
            {
                var insertConversation = await _session.PrepareAsync(
                    "INSERT INTO conversations (" +
                    "id, " +
                    "connectionid, " +
                    "useroneid, " +
                    "usertwoid, " +
                    "status, " +
                    "createdat, " +
                    "updatedat) VALUES(?, ?, ?, ?, ?, ?, ?)");

                var conversation = new Conversation
                {
                    Id = Guid.NewGuid().ToString(),
                    ConnectionId = Context.ConnectionId,
                    UserOneId = conn.SenderId,
                    UserTwoId = conn.ReceiverId,
                    Status = "ACTIVE",
                    CreatedAt = DateTime.UtcNow.ToString(),
                    UpdatedAt = DateTime.UtcNow.ToString(),
                };

                var result = await _session.ExecuteAsync(insertConversation.Bind(
                        conversation.Id,
                        conversation.ConnectionId,
                        conversation.UserOneId,
                        conversation.UserTwoId,
                        conversation.Status,
                        conversation.CreatedAt,
                        conversation.UpdatedAt));

                if (result != null)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, conversation.Id);

                    var conversationDto = new ConversationDto
                    {
                        Id = conversation.Id,
                        ConnectionId = Context.ConnectionId,
                        Status = conversation.Status,
                        ReceiverId = conn.ReceiverId,
                        ReceiverName = conn.ReceiverName,
                        CreatedAt = DateTime.Parse(conversation.CreatedAt),
                        UpdatedAt = DateTime.Parse(conversation.UpdatedAt),
                    };

                    await Clients.Group(conversation.Id).SendAsync("JoinSpecificChatRoom", conversationDto, $"{conn.SenderName} has created  conversation {conversation.Id}");
                }
            }
            else
            {
                var conversationRow = row1 != null ? row1 : row2;

                var conversationDto = new ConversationDto
                {
                    Id = conversationRow.GetValue<string>("id"),
                    ConnectionId = Context.ConnectionId,
                    Status = conversationRow.GetValue<string>("status"),
                    ReceiverId = conn.ReceiverId,
                    ReceiverName = conn.ReceiverName,
                    CreatedAt = DateTime.Parse(conversationRow.GetValue<string>("createdat")),
                    UpdatedAt = DateTime.Parse(conversationRow.GetValue<string>("updatedat")),
                };

                await Groups.AddToGroupAsync(Context.ConnectionId, conversationDto.Id);
                await Clients.Group(conversationDto.Id).SendAsync("JoinSpecificChatRoom", conversationDto, $"{conn.SenderName} has joined  conversation {conversationDto.Id}");
            }
        }

        public async Task SendMessage(SendMessageRequestDto request)
        {
            var getConversation = await _session.PrepareAsync("SELECT * FROM conversations WHERE id = ? ALLOW FILTERING");
            var row = (await _session.ExecuteAsync(getConversation.Bind(request.ConversationId))).FirstOrDefault();

            if (row != null)
            {
                await Clients.Group(request.ConversationId).SendAsync("ReceiveSpecificMessage", request.ConversationId, request.SenderId, request.Content);

                var insertMessage = await _session.PrepareAsync("INSERT INTO messages (id,userid,conversationId,content,status,createdat,updatedat) VALUES (?, ?, ?, ?, ?, ?, ?)");

                var updateConversation = _session.Prepare("UPDATE conversations SET updatedat = ? WHERE id = ? ALLOW FILTERING");

                var message = new Message
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = request.SenderId,
                    ConversationId = request.ConversationId,
                    Content = request.Content,
                    Status = "ACTIVE",
                    CreatedAt = DateTime.UtcNow.ToString(),
                    UpdatedAt = DateTime.UtcNow.ToString(),
                };

                await _session.ExecuteAsync(insertMessage.Bind(
                        message.Id,
                        message.UserId,
                        message.ConversationId,
                        message.Content,
                        message.Status,
                        message.CreatedAt,
                        message.UpdatedAt));

                await _session.ExecuteAsync(updateConversation.Bind(DateTime.UtcNow.ToString(), request.ConversationId));
            }
        }
    }
}
