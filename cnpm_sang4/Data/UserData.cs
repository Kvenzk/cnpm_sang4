using cnpm_sang4.Models;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace cnpm_sang4.Data
{
    public static class UserData
    {
        private static string jsonPath = "Data/users.json";

        public static Dictionary<string, UserModel> GetUsers()
        {
            if (!File.Exists(jsonPath)) return new Dictionary<string, UserModel>();
            var json = File.ReadAllText(jsonPath);
            return JsonSerializer.Deserialize<Dictionary<string, UserModel>>(json);
        }
    }
}
