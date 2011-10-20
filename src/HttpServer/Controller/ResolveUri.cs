using System;
using HttpServer.Model.Entities;

namespace HttpServer.Controller
{
    public class ResolveUri
    {
        public static string For(CurricularUnit uc)
        {
            return String.Format("/fucs/{0}", uc.Key);
        }

        public static string ForFucs()
        {
            return "/fucs";
        }

        public static string ForRoot()
        {
            return "/";
        }
    }
}
