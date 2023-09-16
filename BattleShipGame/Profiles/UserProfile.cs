using AutoMapper;
using BattleShipGame.Database.Models;
using BattleShipGame.DTO;

namespace BattleShipGame.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>();
    }
}