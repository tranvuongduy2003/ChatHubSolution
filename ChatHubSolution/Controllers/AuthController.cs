using Cassandra.Data.Linq;
using ChatHubSolution.Data.Entities;
using ChatHubSolution.DTOs;
using ChatHubSolution.Helpers;
using ChatHubSolution.Implementation.Interfaces;
using ChatHubSolution.Services;
using EventHubSolution.ViewModels.Systems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace ChatHubSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly Cassandra.ISession _session;

        public AuthController(ITokenService tokenService, ICassandraProvider provider)
        {
            _tokenService = tokenService;
            _session = provider.GetSession();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserCreateDto request)
        {
            var getUser = await _session.PrepareAsync("SELECT * FROM users WHERE email = ? ALLOW FILTERING");
            var row = (await _session.ExecuteAsync(getUser.Bind(request.Email))).FirstOrDefault();
            if (row != null)
                return BadRequest(new ApiBadRequestResponse("Email already exists"));

            var insertUser = await _session.PrepareAsync("INSERT INTO users (id, name, email, password, createdat, updatedat) VALUES (?, ?, ?, ?, ?, ?)");

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow.ToString(),
                UpdatedAt = DateTime.UtcNow.ToString(),
            };

            var result = await _session.ExecuteAsync(insertUser.Bind(
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Password,
                    user.CreatedAt,
                    user.UpdatedAt));

            if (result != null)
            {
                //TODO: If user was found, generate JWT Token
                var accessToken = await _tokenService.GenerateAccessTokenAsync(user);

                SignInResponseDto signUpResponse = new SignInResponseDto()
                {
                    AccessToken = accessToken
                };

                return Ok(new ApiOkResponse(signUpResponse));
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse(""));
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDto request)
        {
            var getUser = await _session.PrepareAsync("SELECT * FROM users WHERE email = ? ALLOW FILTERING");
            var row = (await _session.ExecuteAsync(getUser.Bind(request.Email))).FirstOrDefault();
            if (row == null)
                return NotFound(new ApiNotFoundResponse("Invalid credentials"));

            var user = new User
            {
                Id = row.GetValue<string>("id"),
                Email = row.GetValue<string>("email"),
                Name = row.GetValue<string>("name"),
                Password = row.GetValue<string>("password")
            };

            bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (isValid == false)
            {
                return Unauthorized(new ApiUnauthorizedResponse("Invalid credentials"));
            }

            //TODO: If user was found, generate JWT Token
            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);

            SignInResponseDto signInResponse = new SignInResponseDto()
            {
                AccessToken = accessToken
            };

            return Ok(new ApiOkResponse(signInResponse));
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (accessToken == null || accessToken == "")
            {
                return Unauthorized(new ApiUnauthorizedResponse("Unauthorized"));
            }
            var principal = _tokenService.GetPrincipalFromToken(accessToken);
            var userId = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var getUser = await _session.PrepareAsync("SELECT * FROM users WHERE id = ? ALLOW FILTERING");
            var row = (await _session.ExecuteAsync(getUser.Bind(userId))).FirstOrDefault();
            if (row == null)
            {
                return Unauthorized(new ApiUnauthorizedResponse("Unauthorized"));
            }

            var userVm = new UserDto()
            {
                Id = row.GetValue<string>("id"),
                Email = row.GetValue<string>("email"),
                Name = row.GetValue<string>("name"),
                CreatedAt = DateTime.Parse(row.GetValue<string>("createdat")),
                UpdatedAt = DateTime.Parse(row.GetValue<string>("updatedat")),
            };

            return Ok(new ApiOkResponse(userVm));
        }
    }
}
