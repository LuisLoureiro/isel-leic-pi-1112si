using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpServer
{
    public class User
    {
        private static readonly IDictionary<string, string> Users = new Dictionary<string, string>();
        private static readonly IDictionary<string, string[]> UserRoles = new Dictionary<string, string[]>();

        public static void AddUser(string user, string pass, params string[] roles)
        {
            // Não são permitidas passwords vazias ou a null
            if (string.IsNullOrEmpty(pass))
                throw new InvalidOperationException("Não são permitidas chaves de acesso sem valor.");

            // Se user já existir envia excepção
            if (Users.ContainsKey(user))
                throw new InvalidOperationException("Esse utilizador já existe.");

            Users.Add(user, pass);
            UserRoles.Add(user, roles);
        }

        public static void UpdatePassword(string user, string pass)
        {
            // Não são permitidas passwords vazias ou a null
            if (string.IsNullOrEmpty(pass))
                throw new InvalidOperationException("Não são permitidas chaves de acesso sem valor.");

            // Envia excepção se o utilizador não existir
            string value;
            if (!Users.TryGetValue(user, out value))
                throw new InvalidOperationException("Utilizador inexistente.");

            Users[user] = pass;
        }

        public static void AddRoles(string user, params string[] roles)
        {
            // Envia excepção se o utilizador não existir
            string value;
            if (!Users.TryGetValue(user, out value))
                throw new InvalidOperationException("Utilizador inexistente.");

            List<string> newRoles = UserRoles[user].ToList();
            newRoles.AddRange(UserRoles[user]);

            UserRoles[user] = newRoles.ToArray();
        }

        public static void UpdateRoles(string user, params string[] roles)
        {
            // Envia excepção se o utilizador não existir
            string value;
            if (!Users.TryGetValue(user, out value))
                throw new InvalidOperationException("Utilizador inexistente.");

            UserRoles[user] = roles;
        }

        public static string[] GetRoles(string user)
        {
            // Envia excepção se o utilizador não existir
            string value;
            if (!Users.TryGetValue(user, out value))
                throw new InvalidOperationException("Utilizador inexistente.");

            return UserRoles[user];
        }

        public static bool IsCorrect(string user, string pass)
        {
            // Envia excepção se o utilizador não existir
            string value;
            if (!Users.TryGetValue(user, out value))
                throw new InvalidOperationException("Utilizador inexistente.");

            return value.Equals(pass);
        }
    }
}
