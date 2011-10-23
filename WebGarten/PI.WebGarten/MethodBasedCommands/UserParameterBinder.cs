using System;
using System.Reflection;
using System.Security.Principal;

namespace PI.WebGarten.MethodBasedCommands
{
    public class UserParameterBinder : IParameterBinder
    {
        public Func<RequestInfo, object> TryGetBinder(ParameterInfo pi, HttpCmdAttribute attr)
        {
            if (pi.ParameterType == typeof(IPrincipal))
            {
                return ri => ri.User;
            }

            return null;
        }
    }
}
