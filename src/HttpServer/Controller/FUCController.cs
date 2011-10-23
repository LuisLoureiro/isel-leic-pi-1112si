using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Principal;
using HttpServer.Model.Entities;
using HttpServer.Model.Repository;
using HttpServer.Views;
using PI.WebGarten;
using PI.WebGarten.MethodBasedCommands;
using System.Linq;

namespace HttpServer.Controller
{
    public class FucController
    {
        private readonly IRepository<string, CurricularUnit> _repo;

        public FucController()
        {
            _repo = RepositoryLocator.Get<string, CurricularUnit>();
        }

        [HttpCmd(HttpMethod.Get, "/fucs")]
        public HttpResponse GetFucs()
        {
            return new HttpResponse(HttpStatusCode.OK, new FucsView(_repo.GetAll()));
        }

        [HttpCmd(HttpMethod.Get, "/fucs/{acr}")]
        public HttpResponse GetFuc(string acr, IPrincipal principal)
        {
            CurricularUnit fuc;

            try
            {
                fuc = _repo.GetById(acr);
            }
            catch (Exception)
            {
                return new HttpResponse(HttpStatusCode.NotFound);
            }

            return new HttpResponse(HttpStatusCode.OK, new FucsView(fuc, principal));
        }

        [HttpCmd(HttpMethod.Get, "/fucs/new")]
        public HttpResponse GetCreateFuc()
        {
            return new HttpResponse(HttpStatusCode.OK, new ProposalView());
        }

        [HttpCmd(HttpMethod.Get, "/fucs/{acr}/edit")]
        public HttpResponse GetEditFucForm(string acr)
        {
            var fuc = _repo.GetById(acr);

            return new HttpResponse(HttpStatusCode.OK, new ProposalView(fuc));
        }

        [HttpCmd(HttpMethod.Post, "/fucs/{acr}/edit")]
        public HttpResponse PostEditFucForm(string acr, IEnumerable<KeyValuePair<string, string>> content, IPrincipal principal)
        {
            float ects;
            if (!float.TryParse(content.Where(p => p.Key.Equals("ects")).FirstOrDefault().Value, out ects))
                throw new ArgumentException("Valor de ECTS inválido");

            var uc = new CurricularUnit(content.Where(p => p.Key.Equals("name")).FirstOrDefault().Value,
                                        content.Where(p => p.Key.Equals("acr")).FirstOrDefault().Value,
                                        content.Where(p => p.Key.Equals("tipoObrig")).FirstOrDefault().Value.Equals("obrigatoria"),
                                        TranslateSemester(content),
                                        ects);
            var prop = new Proposal(uc, principal.Identity.Name);
            RepositoryLocator.Get<long, Proposal>().Insert(prop);

            return new HttpResponse(HttpStatusCode.SeeOther).WithHeader("Location", ResolveUri.For(prop));
        }

        private static ushort TranslateSemester(IEnumerable<KeyValuePair<string, string>> content)
        {
            ushort val = 0x00;

            for(int i = 1; i<=10; i++)
                if (content.Select(k => k.Key.Equals("" + i)) != null)
                    val = (ushort)(val | (0x01 << (i - 1)));

            return val;
        }
    }
}
