using System;
using System.Collections.Generic;

namespace mvc.Models
{
    public class MvcNotMembershipProvider
    {
        private readonly static IDictionary<int, InternalUser> Users;

        static MvcNotMembershipProvider()
        {
            Users = new Dictionary<int, InternalUser>
                        {
                            {
                                1, new InternalUser
                                       {
                                           Number = 1,
                                           Name = "Administrador",
                                           IsActivated = true,
                                           Password = "2722632186"
                                       }
                                }
                        };
        }

        public static void CreateUser(int number, string nome, string password, string email)
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

        public static bool DeleteUser(int number)
        {
            CheckUser(number);

            return Users.Remove(number);
        }

        public static void ChangePassword(int number, string password)
        {
            CheckUser(number);

            Users[number].ChangePassword(password);
        }

        public static void ActivateUser(int number)
        {
            CheckUser(number);

            Users[number].IsActivated = true;
        }

        public static bool ValidateUser(int number, string password)
        {
            CheckUser(number);

            return Users[number].Password == password;
        }

        public static AccountUser GetUser(int number)
        {
            CheckUser(number);

            return Users[number];
        }

        public static IEnumerable<InternalUser> GetAllUsers()
        {
            return Users.Values;
        }

        private static void CheckUser(int number)
        {
            if (!Users.ContainsKey(number))
                throw new ArgumentException(string.Format("The user with number {0} doesn't exist.", number));
        }
    }
}