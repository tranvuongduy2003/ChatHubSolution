using ChatHubSolution.DTOs;
using ChatHubSolution.Helpers;
using ChatHubSolution.Implementation.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatHubSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly Cassandra.ISession _session;

        public MessagesController(ICassandraProvider provider)
        {
            _session = provider.GetSession();
        }

        [HttpGet("{conversationId}")]
        public async Task<IActionResult> GetMessagesByConversationId(string conversationId)
        {
            var messageDtos = new List<MessageDto>();
            var getMessage = await _session.PrepareAsync("SELECT * FROM messages WHERE conversationid = ? ALLOW FILTERING");
            var rows = await _session.ExecuteAsync(getMessage.Bind(conversationId));

            foreach (var row in rows)
            {
                messageDtos.Add(new MessageDto()
                {
                    Id = row.GetValue<string>("id"),
                    Content = row.GetValue<string>("content"),
                    ConversationId = row.GetValue<string>("conversationid"),
                    UserId = row.GetValue<string>("userid"),
                    Status = row.GetValue<string>("status"),
                    CreatedAt = DateTime.Parse(row.GetValue<string>("createdat")),
                    UpdatedAt = DateTime.Parse(row.GetValue<string>("updatedat")),
                });
            }

            return Ok(new ApiOkResponse(messageDtos.OrderBy(m => m.CreatedAt).ToList()));
        }
    }
}
