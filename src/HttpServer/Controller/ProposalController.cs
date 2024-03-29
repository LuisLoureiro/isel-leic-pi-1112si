﻿using System;
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

        [HttpCmd(HttpMethod.Get, "/props")]
        public HttpResponse GetFucProposal(IPrincipal principal)
        {
            var props = _repo.GetAll().Where(p => p.State == AbstractEntity<long>.Status.Pending);

            return new HttpResponse(HttpStatusCode.OK, 
                new ProposalView(principal.IsInRole(Roles.Utilizador) ? 
                                    props.Where(p => p.Owner.Equals(principal.Identity.Name)) : 
                                    props));
        }

        [HttpCmd(HttpMethod.Get, "/props/{id}")]
        public HttpResponse GetFucProposal(long id, IPrincipal principal)
        {
            var prop = _repo.GetById(id);

            return prop == null ?
                new HttpResponse(HttpStatusCode.NotFound, new Handler.NotFound()) : 
                new HttpResponse(HttpStatusCode.OK, new ProposalView(prop, principal));
        }

        [HttpCmd(HttpMethod.Post, "/props/{id}/accept")]
        public HttpResponse PostAcceptFucProposal(long id)
        {
            var prop = _repo.GetById(id);

            // Verificar se esta proposta corresponde a uma nova FUC ou a uma actualização de uma FUC existente.
            IRepository<string, CurricularUnit> ucRepo = RepositoryLocator.Get<string, CurricularUnit>();
            
            if (ucRepo.GetById(prop.Info.Key) == null) // Nova FUC
                ucRepo.Insert(prop.Info);
            else
                ucRepo.Update(prop.Info);

            // Actualiza o estado da proposta
            prop.UpdateStatus(AbstractEntity<long>.Status.Accepted);

            return new HttpResponse(HttpStatusCode.SeeOther).WithHeader("Location", ResolveUri.For(prop.Info));
        }

        [HttpCmd(HttpMethod.Post, "/props/{id}/cancel")]
        public HttpResponse PostCancelFucProposal(long id)
        {
            // Actualiza o estado da proposta
            _repo.GetById(id).UpdateStatus(AbstractEntity<long>.Status.Canceled);

            return new HttpResponse(HttpStatusCode.SeeOther).WithHeader("Location", ResolveUri.ForProposals());
        }

        [HttpCmd(HttpMethod.Get, "/props/{id}/edit")]
        public HttpResponse GetEditFucProposal(long id)
        {
            var prop = _repo.GetById(id);

            return prop == null ? 
                new HttpResponse(HttpStatusCode.NotFound, new Handler.NotFound()) :
                new HttpResponse(HttpStatusCode.OK, new ProposalView(prop.Info, ResolveUri.ForEdit(prop)));
        }

        [HttpCmd(HttpMethod.Post, "/props/{id}/edit")]
        public HttpResponse PostEditFucProposal(long id, IEnumerable<KeyValuePair<string, string>> content, IPrincipal principal)
        {
            float ects;
            if (!float.TryParse(content.Where(p => p.Key.Equals("ects")).FirstOrDefault().Value, out ects))
                throw new ArgumentException("Valor de ECTS inválido");

            Proposal prop = _repo.GetById(id);
            CurricularUnit uc = prop.Info;

            // Alterar a unidade curricular associada à proposta
            uc.Name = content.Where(p => p.Key.Equals("name")).FirstOrDefault().Value;
            uc.Mandatory = content.Where(p => p.Key.Equals("tipoObrig")).FirstOrDefault().Value.Equals("obrigatoria");
            uc.Semester = Utils.TranslateSemester(content);
            uc.Ects = ects;
            uc.Results = content.Where(p => p.Key.Equals("results")).FirstOrDefault().Value;
            uc.Objectives = content.Where(p => p.Key.Equals("objectives")).FirstOrDefault().Value;
            uc.Assessment = content.Where(p => p.Key.Equals("assessment")).FirstOrDefault().Value;
            uc.Program = content.Where(p => p.Key.Equals("program")).FirstOrDefault().Value;

            // Actualizar as precedências
            uc.UpdatePrecedences(Utils.RetrievePrecedencesFromPayload(content)); 

            return new HttpResponse(HttpStatusCode.SeeOther).WithHeader("Location", ResolveUri.For(prop));
        }
    }
}
