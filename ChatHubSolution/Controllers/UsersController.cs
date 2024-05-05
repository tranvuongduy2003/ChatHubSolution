using Cassandra;
using ChatHubSolution.DTOs;
using ChatHubSolution.Helpers;
using ChatHubSolution.Implementation.Interfaces;
using ChatHubSolution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace ChatHubSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Cassandra.ISession _session;
        private readonly ITokenService _tokenService;

        public UsersController(ICassandraProvider provider, ITokenService tokenService)
        {
            _session = provider.GetSession();
            _tokenService = tokenService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] string? search)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (accessToken == null || accessToken == "")
            {
                return Unauthorized(new ApiUnauthorizedResponse("Unauthorized"));
            }
            var principal = _tokenService.GetPrincipalFromToken(accessToken);
            var userId = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var userDtos = new List<UserDto>();
            var rows = (await _session.ExecuteAsync(new SimpleStatement("SELECT * FROM users"))).Where(r => r.GetValue<string>("id") != userId);

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
