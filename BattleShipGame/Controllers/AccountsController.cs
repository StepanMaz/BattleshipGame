using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

using BattleShipGame.Services;
using BattleShipGame.DTO;
using BattleShipGame.Database.Models;
using AutoMapper;
using BattleShipGame.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BattleShipGame.Controllers;

[ApiController, Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAuthService authSerivce;
    private readonly IMapper mapper;
    private readonly ILogger<AccountsController> logger;

    public AccountsController(
        IAuthService authSerivce,
        IMapper mapper,
        ILogger<AccountsController> logger)
    {
        this.authSerivce = authSerivce;
        this.mapper = mapper;
        this.logger = logger;
    }

    [HttpPut("Login")]
    public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO loginDTO)
    {
        var user = await authSerivce.Login(loginDTO.Login, loginDTO.Password);

        if(user is null)
        {
            return Unauthorized("Incorrect login or password");
        }

        SingIn(user);

        return Ok(mapper.Map<UserDTO>(user));
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO loginDTO)
    {
        User user;
        try
        {
            user = await authSerivce.Register(loginDTO.Login, loginDTO.Password);
        }
        catch (PasswordException e)
        {
            return BadRequest(e.Message);
        }
        catch(DbUpdateException)
        {
            return BadRequest("Login is already in use");
        }

        SingIn(user);

        return Ok(mapper.Map<UserDTO>(user));
    }

    private async void SingIn(User user)
    {
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim("UserId", user.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
    }
}