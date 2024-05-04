using Cassandra;
using ChatHubSolution.DTOs;
using ChatHubSolution.Helpers;
using ChatHubSolution.Implementation.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ChatHubSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Cassandra.ISession _session;

        public UsersController(ICassandraProvider provider)
        {
            _session = provider.GetSession();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] string? search)
        {
            var userDtos = new List<UserDto>();
            var rows = await _session.ExecuteAsync(new SimpleStatement("SELECT * FROM users"));

            foreach (var row in rows)
            {
                userDtos.Add(new UserDto()
                {
                    Id = row.GetValue<string>("id"),
                    Email = row.GetValue<string>("email"),
                    Name = row.GetValue<string>("name"),
                    CreatedAt = DateTime.Parse(row.GetValue<string>("createdat")),
                    UpdatedAt = DateTime.Parse(row.GetValue<string>("updatedat")),
                });
            }

            if (!search.IsNullOrEmpty())
                userDtos.Where(u => u.Name.ToLower().Contains(search.ToLower()));

            return Ok(new ApiOkResponse(userDtos));
        }
    }
}
