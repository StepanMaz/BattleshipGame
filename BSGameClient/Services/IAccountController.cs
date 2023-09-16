using BattleShipGameClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSGameClient.Services
{
    public interface IAccountController
    {
        public Task<UserDTO?> Login(string login, string password);
        public Task<UserDTO> Register(string login, string password);
    }
}
