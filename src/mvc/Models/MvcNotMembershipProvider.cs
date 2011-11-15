using System;
using System.Collections.Generic;

namespace mvc.Models
{
    public class MvcNotMembershipProvider
    {
        private readonly static IDictionary<string, InternalUser> Users;

        static MvcNotMembershipProvider()
        {
            Users = new Dictionary<string, InternalUser>
                        {
                            {
                                "1", new InternalUser
                                       {
                                           Number = "1",
                                           Name = "Administrador",
                                           IsActivated = true,
                                           Password = "2722632186"
                                       }
                                }
                        };
        }

        public static void CreateUser(string number, string nome, string password, string email)
        {
            if (Users.ContainsKey(number))
                throw new ArgumentException("User already exists.");

            Users[number] = new InternalUser
                                {
                                    Number = number, Name = nome, Password = password, 
                                    ConfirmPassword = password, Email = email
                                };
        }

        public static void CreateUser(DefaultUser user)
        {
            CreateUser(user.Number, user.Name, user.Password, user.Email);
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

            Users[number].ChangePassword(password);
        }

        public static void ActivateUser(string number)
        {
            CheckUser(number);

            Users[number].IsActivated = true;
        }

        public static bool ValidateUser(string number, string password)
        {
            CheckUser(number);

            return Users[number].Password == password;
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
    }
}