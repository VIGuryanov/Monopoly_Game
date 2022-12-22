using MonopolyMAUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonopolyMAUI.Services
{
    public class PlayerService
    {
        List<User> UserList { get; set; }

        public async Task<List<User>> GetPlayersFromJsonAsync()
        {
            if (UserList?.Count > 0)
                return UserList;
            //TODO: Переделать под логику сервера и клиента(не забудь зарегистрировать)
            using var stream = await FileSystem.OpenAppPackageFileAsync("userdata.json");
            /*using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();*/
            UserList = await JsonSerializer.DeserializeAsync<List<User>>(stream);
            return UserList;
        }
    }
}
