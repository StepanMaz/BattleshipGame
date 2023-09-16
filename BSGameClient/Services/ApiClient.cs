using BattleShipGameClient.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json;

namespace BSGameClient.Services
{
    public class ApiClient : IAccountController, IGameController
    {
        private readonly HttpClient httpClient;

        public ApiClient(Uri base_uri)
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer};
            httpClient = new HttpClient(handler) { BaseAddress = base_uri };
        }

        public async Task<UserDTO?> Login(string login, string password)
        {
            var responce = await httpClient.PutAsJsonAsync("accounts/login", new { Login = login, Password = password});

            if(responce.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<UserDTO?>(await responce.Content.ReadAsStringAsync())!;
            }

            return null;
        }

        public async Task<UserDTO> Register(string login, string password)
        {
            var responce = await httpClient.PostAsJsonAsync("accounts/register", new { Login = login, Password = password });
            var message = await responce.Content.ReadAsStringAsync();

            if (responce.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<UserDTO?>(message)!;
            }

            throw new Exception(message);
        }

        public async Task<Guid> CreateLobby()
        {
            var responce = await httpClient.PostAsync("game/lobbies", null);
            var json = await responce.Content.ReadAsStringAsync();
            var lobbyData = JsonConvert.DeserializeObject<LobbyData>(json);
            if (lobbyData is null) throw new Exception();
            return lobbyData.Id;
        }

        public async Task<IEnumerable<Guid>> GetAllLobies()
        {
            return (await httpClient.GetFromJsonAsync<IEnumerable<Guid>>("game/lobbies")) ?? Enumerable.Empty<Guid>();
        }
    }
}
