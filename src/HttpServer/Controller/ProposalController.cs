using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using HttpServer.Model.Entities;
using HttpServer.Model.Repository;
using HttpServer.Views;
using PI.WebGarten;
using PI.WebGarten.MethodBasedCommands;

namespace HttpServer.Controller
{
    class ProposalController
    {
        private readonly IRepository<long, Proposal> _repo;

        public ProposalController()
        {
            _repo = RepositoryLocator.Get<long, Proposal>();
        }

        [HttpCmd(HttpMethod.Post, "/fucs/new")]
        public HttpResponse PostCreateFuc(IEnumerable<KeyValuePair<string, string>> content, IPrincipal principal)
        {
            var uc = BuildCurricularUnitFromContent(content);
            var prop = new Proposal(uc, principal.Identity.Name);
            _repo.Insert(prop);

            return new HttpResponse(HttpStatusCode.SeeOther).WithHeader("Location", ResolveUri.For(prop));
        }

        [HttpCmd(HttpMethod.Get, "/props")]
        public HttpResponse GetFucProposal()
        {
            return new HttpResponse(HttpStatusCode.OK, new ProposalView(_repo.GetAll()));
        }

        [HttpCmd(HttpMethod.Get, "/props/{id}")]
        public HttpResponse GetFucProposal(string acr, long id, IPrincipal principal)
        {
            return new HttpResponse(HttpStatusCode.OK, new FucsView(_repo.GetById(id).Info, principal));
        }

        [HttpCmd(HttpMethod.Post, "/props/{id}/accept")]
        public HttpResponse PostAcceptFucProposal(string acr, long id)
        {
            var prop = _repo.GetById(id);

            RepositoryLocator.Get<string, CurricularUnit>().Insert(prop.Info);

            return new HttpResponse(HttpStatusCode.SeeOther).WithHeader("Location", ResolveUri.For(prop.Info));
        }

        [HttpCmd(HttpMethod.Post, "/props/{id}/cancel")]
        public HttpResponse PostCancelFucProposal(string acr, long id)
        {
            //TODO 

            return new HttpResponse(HttpStatusCode.SeeOther).WithHeader("Location", ResolveUri.ForProposals());
        }

        [HttpCmd(HttpMethod.Get, "/props/{id}/edit")]
        public HttpResponse GetEditFucProposal(string acr, long id)
        {
            return null;
        }

        private static CurricularUnit BuildCurricularUnitFromContent(IEnumerable<KeyValuePair<string, string>> content)
        {
            float ects;
            float.TryParse(content.Where(p => p.Key == "tipoObrig").FirstOrDefault().Value, out ects);

            var uc = new CurricularUnit(content.Where(p => p.Key == "name").FirstOrDefault().Value,
                                        content.Where(p => p.Key == "acr").FirstOrDefault().Value,
                                        content.Where(p => p.Key == "tipoObrig").FirstOrDefault().Value.Equals("obrigatoria"),
                                        0,
                                        ects
                                        );

            uc.Assessment = content.Where(p => p.Key == "assessment").FirstOrDefault().Value;
            uc.Objectives = content.Where(p => p.Key == "objective").FirstOrDefault().Value;
            uc.Results = content.Where(p => p.Key == "results").FirstOrDefault().Value;
            uc.Program = content.Where(p => p.Key == "program").FirstOrDefault().Value;

            return uc;
        }
    }
}
