using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSGameClient.Services
{
    public interface IGameController
    {
        Task<Guid> CreateLobby();
        Task<IEnumerable<Guid>> GetAllLobies();
    }
}
