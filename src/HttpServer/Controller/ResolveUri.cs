﻿using System;
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
            return String.Format("/props/{0}", prop.Key);
        }

        public static string ForEdit(CurricularUnit fuc)
        {
            return String.Format("/fucs/{0}/edit", fuc.Key);
        }

        public static string ForEdit(Proposal prop)
        {
            return String.Format("/props/{0}/edit", prop.Key);
        }

        public static string ForLogin()
        {
            return String.Format("/login");
        }

        public static string ForNewFuc()
        {
            return string.Format("/fucs/new");
        }

        public static string ForProposalAccept(long id)
        {
            return string.Format("/props/{0}/accept", id);
        }

        public static string ForProposalCancel(long id)
        {
            return string.Format("/props/{0}/cancel", id);
        }
    }
}
