using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace mvc.Models
{
    public class MvcRoleProvider : RoleProvider
    {
        private static readonly IDictionary<string, List<string>> RolesUsers;

        static MvcRoleProvider()
        {
            RolesUsers = new Dictionary<string, List<string>>
                             {
                                 {
                                     "admin", new List<string>
                                                  {
                                                      "1"
                                                  }
                                     },
                                 {
                                     "default", new List<string>()
                                     }
                             };
        }

        public override string ApplicationName { get; set; }

        public override bool RoleExists(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be nothing or empty string.");

            return RolesUsers.ContainsKey(roleName);
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            CheckRoleExistance(roleName);

            return RolesUsers[roleName].Contains(username);
        }

        public override string[] GetRolesForUser(string username)
        {
            return (RolesUsers.Where(roleUsers => roleUsers.Value.Contains(username)).Select(roleUsers => roleUsers.Key)).ToArray();
        }

        public override void CreateRole(string roleName)
        {
            RolesUsers[roleName] = new List<string>();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            CheckRoleExistance(roleName);

            if (throwOnPopulatedRole && RolesUsers[roleName].Count > 0)
                throw new InvalidOperationException("The role indicated contains users. Cannot delete!");

            return RolesUsers.Remove(roleName);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (RoleExists(roleName))
                {
                    RolesUsers[roleName].AddRange(usernames);
                }
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (RoleExists(roleName))
                {
                    RolesUsers[roleName].RemoveAll(user => usernames.Contains(user));
                }
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            CheckRoleExistance(roleName);

            return RolesUsers[roleName].ToArray();
        }

        public override string[] GetAllRoles()
        {
            return RolesUsers.Keys.ToArray();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            CheckRoleExistance(roleName);

            return RolesUsers[roleName].Where(
                str => str.Contains(usernameToMatch)
                ).ToArray();
        }
    
        private void CheckRoleExistance(string roleName)
        {
            if (!RoleExists(roleName))
                throw new ArgumentException(string.Format("The role {0} doesn't exist.", roleName));
        }
    }
}