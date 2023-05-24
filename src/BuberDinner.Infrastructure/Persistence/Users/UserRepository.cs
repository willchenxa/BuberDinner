using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.User;

using Microsoft.EntityFrameworkCore;

namespace BuberDinner.Infrastructure.Persistence.Users;

public class UserRepository : IUserRepository
{
    private readonly BuberDinnerDbContext _userDbContext;

    public UserRepository(BuberDinnerDbContext userDbContext)
    {
        _userDbContext = userDbContext;
    }

    public async Task Add(User user)
    {
        await _userDbContext.AddAsync(user);
        await _userDbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var result =  await _userDbContext.Users.FirstOrDefaultAsync(user =>
            user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        return result;
    }
};