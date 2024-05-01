using ChatHubSolution.Constants;
using ChatHubSolution.Data;
using ChatHubSolution.Data.Entities;
using ChatHubSolution.DTOs;
using ChatHubSolution.Helpers;
using ChatHubSolution.Services;
using EventHubSolution.ViewModels.Systems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace ChatHubSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _db;

        public AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, ITokenService tokenService, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _db = db;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserCreateDto request)
        {
            var useByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (useByEmail != null)
                return BadRequest(new ApiBadRequestResponse("Email already exists"));

            User user = new()
            {
                Email = request.Email,
                FullName = request.FullName,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var userToReturn = await _userManager.FindByEmailAsync(request.Email);

                await _userManager.AddToRoleAsync(user, "USER");

                await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

                //TODO: If user was found, generate JWT Token
                var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
                var refreshToken = await _userManager.GenerateUserTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH);

                await _userManager.SetAuthenticationTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH, refreshToken);

                SignInResponseDto signUpResponse = new SignInResponseDto()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };

                return CreatedAtAction(nameof(SignUp), new { id = userToReturn.Id }, signUpResponse);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse(result));
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return NotFound(new ApiNotFoundResponse("Invalid credentials"));

            bool isValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (isValid == false)
            {
                return Unauthorized(new ApiUnauthorizedResponse("Invalid credentials"));
            }

            await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

            //TODO: If user was found, generate JWT Token
            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = await _userManager.GenerateUserTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH);

            await _userManager.SetAuthenticationTokenAsync(user, TokenProviders.DEFAULT, TokenTypes.REFRESH, refreshToken);

            SignInResponseDto signInResponse = new SignInResponseDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
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

            var user = await _userManager.FindByIdAsync(principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value);
            if (user == null)
            {
                return Unauthorized(new ApiUnauthorizedResponse("Unauthorized"));
            }

            var userVm = new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return Ok(new ApiOkResponse(userVm));
        }
    }
}
