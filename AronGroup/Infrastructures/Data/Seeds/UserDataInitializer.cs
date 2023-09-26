using AronGroup.Infrastructures.Data.Repositories.Abstractions;
using AronGroup.Infrastructures.Data.Seeds.Abstractions;
using AronGroup.Models.User;

namespace AronGroup.Infrastructures.Data.Seeds;

public class UserDataInitializer : IDataInitializer
{
    private readonly IUserRepository userRepository;

    public UserDataInitializer(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public void InitializeData()
    {
        if (!userRepository.TableNoTracking.Any(p => p.UserName == "Admin"))
        {
            userRepository.Add(new User
            {
                UserName = "Admin",
                Email = "admin@site.com",
                FullName = "Admin",
                Token = Guid.NewGuid(),
                PasswordHash = "AQAAAAIAAYagAAAAELzxNgOdzytfjMVKADVoVIfvSAT7aZ9PBD4Rhtbihqh2haxRlkQ190+gdzotCjZX3Q==" // password = 123456789
            });
        }
    }
}
