using System.Security.Cryptography;
using System.Text;
using BattleShipGame.Database.Models;
using BattleShipGame.Exceptions;

namespace BattleShipGame.Services;

public class AuthProxy : IAuthService
{
    private readonly AuthService authService;
    private readonly SHA512 _encryptor;

    public AuthProxy(AuthService authService)
    {
        this.authService = authService;

        _encryptor = SHA512.Create();
    }

    public Task<User?> Login(string login, string password)
    {
        password = Encrypt(password);

        return authService.Login(login, password);
    }

    public Task<User> Register(string login, string password)
    {
        if(password.Length <= 7) {
            throw new PasswordException("Password is too short");
        }

        if(!password.Any(char.IsUpper)) {
            throw new PasswordException("Password should contain at least one upper letter");
        }

        password = Encrypt(password);

        return authService.Register(login, password);
    }
        
    private string Encrypt(string text)
    {
        var text_byte_array = Encoding.UTF8.GetBytes(text);
        var encoded_byte_array = _encryptor.ComputeHash(text_byte_array);
        return Convert.ToBase64String(encoded_byte_array);
    }
}