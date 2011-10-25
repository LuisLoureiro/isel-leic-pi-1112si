using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Principal;
using HttpServer.Model.Entities;
using HttpServer.Model.Repository;
using HttpServer.Views;
using PI.WebGarten;
using PI.WebGarten.HttpContent.Html;
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
        public HttpResponse GetFucs(IPrincipal principal)
        {
            return new HttpResponse(HttpStatusCode.OK, new FucsView(_repo.GetAll(), principal));
        }

        [HttpCmd(HttpMethod.Get, "/fucs/{acr}")]
        public HttpResponse GetFuc(string acr, IPrincipal principal)
        {
            CurricularUnit fuc = _repo.GetById(acr);

            return fuc == null ? 
                new HttpResponse(HttpStatusCode.NotFound, new TextContent(string.Format("Unidade Curricular {0} inexistente.", acr))) : 
                new HttpResponse(HttpStatusCode.OK, new FucsView(fuc, principal));
        }

        [HttpCmd(HttpMethod.Get, "/fucs/new")]
        public HttpResponse GetCreateFuc()
        {
            return new HttpResponse(HttpStatusCode.OK, new ProposalView());
        }

        [HttpCmd(HttpMethod.Post, "/fucs/new")]
        public HttpResponse PostCreateFuc(IEnumerable<KeyValuePair<string, string>> content, IPrincipal principal)
        {
            return PostEditFucForm(null, content, principal);
        }

        [HttpCmd(HttpMethod.Get, "/fucs/{acr}/edit")]
        public HttpResponse GetEditFucForm(string acr)
        {
            var fuc = _repo.GetById(acr);

            return new HttpResponse(HttpStatusCode.OK, new ProposalView(fuc, ResolveUri.ForEdit(fuc)));
        }

        [HttpCmd(HttpMethod.Post, "/fucs/{acr}/edit")]
        public HttpResponse PostEditFucForm(string acr, IEnumerable<KeyValuePair<string, string>> content, IPrincipal principal)
        {
            // Criar uma unidade curricular, associada à proposta, com os valores recolhidos do formulário de edição.
            var uc = BuildCurricularUnitFromContent(content);
            
            // Criar a proposta de alteração de unidade curricular.
            var prop = new Proposal(uc, principal.Identity.Name);
            RepositoryLocator.Get<long, Proposal>().Insert(prop);

            return new HttpResponse(HttpStatusCode.SeeOther).WithHeader("Location", ResolveUri.For(prop));
        }

        private static CurricularUnit BuildCurricularUnitFromContent(IEnumerable<KeyValuePair<string, string>> content)
        {
            float ects;
            if (!float.TryParse(content.Where(p => p.Key.Equals("ects")).FirstOrDefault().Value, out ects))
                throw new ArgumentException("Valor de ECTS inválido");

            var uc = new CurricularUnit(content.Where(p => p.Key == "name").FirstOrDefault().Value,
                                        content.Where(p => p.Key == "acr").FirstOrDefault().Value,
                                        content.Where(p => p.Key == "tipoObrig").FirstOrDefault().Value.Equals("obrigatoria"),
                                        Utils.TranslateSemester(content),
                                        ects
                                        )
            {
                Assessment = content.Where(p => p.Key == "assessment").FirstOrDefault().Value,
                Objectives = content.Where(p => p.Key == "objectives").FirstOrDefault().Value,
                Results = content.Where(p => p.Key == "results").FirstOrDefault().Value,
                Program = content.Where(p => p.Key == "program").FirstOrDefault().Value
            };

            // Actualizar as precedências
            uc.UpdatePrecedences(Utils.RetrievePrecedencesFromPayload(content)); 

            return uc;
        }
    }
}
