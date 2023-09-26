using AronGroup.Infrastructures.Data.DbContext;
using AronGroup.Infrastructures.Data.Repositories.Abstractions;
using AronGroup.Models.Base;
using AronGroup.Models.User;
using AronGroup.Utilities;
using Microsoft.EntityFrameworkCore;

namespace AronGroup.Infrastructures.Data.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<User?> GetByUserAndPassAsync(string username, string password, CancellationToken cancellationToken)
    {
        var passwordHash = SecurityHelper.GetSha256Hash(password);
        return await Table.Where(p => p.UserName == username && p.PasswordHash == passwordHash).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        return await Table.Where(p => p.Token.ToString() == token).SingleOrDefaultAsync(cancellationToken);
    }
}
