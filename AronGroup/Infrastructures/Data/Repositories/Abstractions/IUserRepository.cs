using AronGroup.Models.Base;
using AronGroup.Models.User;

namespace AronGroup.Infrastructures.Data.Repositories.Abstractions;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUserAndPassAsync(string username, string password, CancellationToken cancellationToken);

    Task<User?> GetByTokenAsync(string token, CancellationToken cancellationToken);
}
