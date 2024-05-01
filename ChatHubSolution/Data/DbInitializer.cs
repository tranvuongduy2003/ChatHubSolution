using Bogus;
using ChatHubSolution.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace ChatHubSolution.Data
{
    public class DbInitializer
    {
        private const int MAX_USERS_QUANTITY = 10;

        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context,
          UserManager<User> userManager,
          RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            SeedRoles().Wait();
            SeedUsers().Wait();
        }

        private async Task SeedRoles()
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "USER",
                    ConcurrencyStamp = "1",
                    NormalizedName = "user"
                });
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                var userFaker = new Faker<User>()
                    .RuleFor(u => u.Id, _ => Guid.NewGuid().ToString())
                    .RuleFor(u => u.Email, f => f.Person.Email)
                    .RuleFor(u => u.NormalizedEmail, f => f.Person.Email)
                    .RuleFor(u => u.UserName, f => f.Person.UserName)
                    .RuleFor(u => u.NormalizedUserName, f => f.Person.UserName)
                    .RuleFor(u => u.LockoutEnabled, f => false)
                    .RuleFor(u => u.FullName, f => f.Person.FullName);

                for (int userIndex = 0; userIndex < MAX_USERS_QUANTITY * 2; userIndex++)
                {
                    var customer = userFaker.Generate();
                    var customerResult = await _userManager.CreateAsync(customer, "User@123");
                    if (customerResult.Succeeded)
                    {
                        var user = await _userManager.FindByEmailAsync(customer.Email);
                        await _userManager.AddToRoleAsync(user, "USER");
                    }
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
