using System.Security.Cryptography;
using System.Text;
using BattleShipGame.Database.Models;
using BattleShipGame.Repositories;

namespace BattleShipGame.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository userRepository;
    private readonly ILogger<AuthService> logger;

    public AuthService(IUserRepository userRepository,
        ILogger<AuthService> logger)
    {
        this.userRepository = userRepository;
        this.logger = logger;
    }

    public async Task<User?> Login(string login, string password)
    {
        var user = await userRepository.GetUser(login, password);

        if (user is null)
        {
            return null;
        }

        logger.LogInformation("LogIn: {0}", user.Login);

        return user;
    }

    public async Task<User> Register(string login, string password)
    {   
        var user = new User() {
            Login = login,
            Password = password
        };

        await userRepository.Create(user);

        logger.LogInformation("Register: {0}", user.Login);

        return user;
    }
}