using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using mvc.Crypto;

namespace mvc.Models
{
    public class MvcNotMembershipProvider
    {
        private readonly static IDictionary<string, InternalUser> Users;
        private static readonly IDictionary<string, string> HashUser;

        static MvcNotMembershipProvider()
        {
            Users = new Dictionary<string, InternalUser>
                        {
                            {
                                "1", new InternalUser
                                         {
                                             Number = "1",
                                             Name = "Administrador",
                                             Email = "isel.leic.pi.li51ng08@gmail.com",
                                             IsActivated = true,
                                             Password = "2722632186".GetHashCode().ToString(),
                                             ConfirmPassword = "2722632186".GetHashCode().ToString()
                                         }
                                },
                            {
                                "2", new InternalUser
                                         {
                                             Number = "2",
                                             Name = "Utilizador",
                                             Email = "Utilizador@email.com",
                                             IsActivated = true,
                                             Password = "123456".GetHashCode().ToString(),
                                             ConfirmPassword = "123456".GetHashCode().ToString()
                                         }
                                }
                        };
            HashUser = new Dictionary<string, string>
                           {
                               {
                                   MD5Crypto.GenerateMD5("1"), "1"
                                   },
                               {
                                   MD5Crypto.GenerateMD5("2"), "2"
                                   }
                           };
        }

        public static string CreateUser(string number, string nome, string password, string email)
        {
            if (Users.ContainsKey(number))
                throw new ArgumentException("User already exists.");

            Users[number] = new InternalUser
                                {
                                    Number = number, Name = nome, Password = password.GetHashCode().ToString(), 
                                    ConfirmPassword = password, Email = email
                                };
            string hash = MD5Crypto.GenerateMD5(number);

            HashUser[hash] = number;

            Roles.AddUserToRole(number, "default");

            return hash;
        }

        public static string CreateUser(DefaultUser user)
        {
            return CreateUser(user.Number, user.Name, user.Password, user.Email);
        }

        public static void UpdateUser(AccountUser user)
        {
            Users[user.Number].Name = user.Name;
            Users[user.Number].Email = user.Email;
            Users[user.Number].Name = user.Name;
            ChangePassword(user.Number, user.Password);
        }

        public static bool DeleteUser(string number)
        {
            CheckUser(number);

            return Users.Remove(number);
        }

        public static void ChangePassword(string number, string password)
        {
            CheckUser(number);
            CheckActivation(number);

            Users[number].ChangePassword(password.GetHashCode().ToString());
        }

        public static void ActivateUser(string hash)
        {
            Users[CheckHashUser(hash)].IsActivated = true;
        }

        public static bool ValidateUser(string number, string password)
        {
            CheckUser(number);
            CheckActivation(number);

            return Users[number].Password == password.GetHashCode().ToString();
        }

        public static AccountUser GetUser(string number)
        {
            CheckUser(number);

            return Users[number];
        }

        public static IEnumerable<InternalUser> GetAllUsers()
        {
            return Users.Values;
        }

        private static void CheckUser(string number)
        {
            if (!Users.ContainsKey(number))
                throw new ArgumentException(string.Format("The user with number {0} doesn't exist.", number));
        }

        private static void CheckActivation(string number)
        {
            if (!Users[number].IsActivated)
                throw new InvalidOperationException(string.Format("The user with number {0} is not activated.", number));
        }

        private static string CheckHashUser(string hash)
        {
            string user;
            if (!HashUser.TryGetValue(hash, out user))
                throw new ArgumentException("The given hash value doesn't match any user.");

            return user;
        }
    }
}