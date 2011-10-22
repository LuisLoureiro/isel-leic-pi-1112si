using System;
using HttpServer.Model.Entities;

namespace HttpServer.Controller
{
    public class ResolveUri
    {
        public static string ForRoot()
        {
            return "/";
        }

        public static string ForFucs()
        {
            return "/fucs";
        }

        public static string For(CurricularUnit uc)
        {
            return String.Format("/fucs/{0}", uc.Key);
        }

        public static string ForProposals()
        {
            return "/props";
        }

        public static string For(Proposal prop)
        {
            return String.Format("/props/{1}", prop.Key);
        }
    }
}
