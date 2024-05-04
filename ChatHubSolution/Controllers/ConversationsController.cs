using ChatHubSolution.DTOs;
using ChatHubSolution.Helpers;
using ChatHubSolution.Implementation.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatHubSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly Cassandra.ISession _session;

        public ConversationsController(ICassandraProvider provider)
        {
            _session = provider.GetSession();
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetConversationsByUserId(string userId)
        {
            var conversationDtos = new List<ConversationDto>();

            var getConversationsByUserOne = await _session.PrepareAsync("SELECT * FROM conversations WHERE useroneid = ? ALLOW FILTERING");
            var getConversationsByUserTwo = await _session.PrepareAsync("SELECT * FROM conversations WHERE usertwoid = ? ALLOW FILTERING");
            var rows1 = await _session.ExecuteAsync(getConversationsByUserOne.Bind(userId));
            var rows2 = await _session.ExecuteAsync(getConversationsByUserTwo.Bind(userId));

            var rows = rows1.Concat(rows2).OrderBy(r => DateTime.Parse(r.GetValue<string>("updatedat")));

            foreach (var row in rows)
            {
                var messageDtos = new List<MessageDto>();
                var conversationId = row.GetValue<string>("id");
                var getMessage = await _session.PrepareAsync("SELECT * FROM messages WHERE conversationid = ? ALLOW FILTERING");
                var messageRows = await _session.ExecuteAsync(getMessage.Bind(conversationId));
                var lastMessageRow = messageRows.MaxBy(m => DateTime.Parse(m.GetValue<string>("createdat")));

                var userOneId = row.GetValue<string>("useroneid");
                var userTwoId = row.GetValue<string>("usertwoid");

                var receiverId = userOneId != userId ? userOneId : userTwoId;
                var getUser = await _session.PrepareAsync("SELECT * FROM users WHERE id = ? ALLOW FILTERING");
                var userRow = (await _session.ExecuteAsync(getUser.Bind(receiverId))).FirstOrDefault();

                conversationDtos.Add(new ConversationDto()
                {
                    Id = row.GetValue<string>("id"),
                    ReceiverId = receiverId,
                    ReceiverName = userRow.GetValue<string>("name"),
                    ConnectionId = row.GetValue<string>("connectionid"),
                    LastMessage = lastMessageRow?.GetValue<string>("content") ?? "",
                    Status = row.GetValue<string>("status"),
                    CreatedAt = DateTime.Parse(row.GetValue<string>("createdat")),
                    UpdatedAt = DateTime.Parse(row.GetValue<string>("updatedat")),
                });
            }

            return Ok(new ApiOkResponse(conversationDtos));
        }
    }
}
