using System;
using System.Net;
using HttpServer.Model.Entities;
using HttpServer.Model.Repository;
using HttpServer.Views;
using PI.WebGarten;
using PI.WebGarten.MethodBasedCommands;

namespace HttpServer.Controller
{
    class ProposalController
    {
        private readonly IRepository<UInt32, Proposal> _repo;

        public ProposalController()
        {
            _repo = RepositoryLocator.Get<UInt32, Proposal>();
        }


        [HttpCmd(HttpMethod.Get, "/props")]
        public HttpResponse GetFucProposal()
        {
            return new HttpResponse(HttpStatusCode.OK, new ProposalView(_repo.GetAll()));
        }

        [HttpCmd(HttpMethod.Get, "/fucs/{acr}/props/{id}")]
        public HttpResponse GetFucProposal(string acr, int id)
        {
            return null;
        }

        [HttpCmd(HttpMethod.Get, "/fucs/{acr}/props")]
        public HttpResponse GetFucProposals(string acr)
        {
            return null;
        }

        [HttpCmd(HttpMethod.Post, "/fucs/{acr}/props/{id}/accept")]
        public HttpResponse PostAcceptFucProposal(string acr, int id)
        {
            return null;
        }

        [HttpCmd(HttpMethod.Post, "/fucs/{acr}/props/{id}/cancel")]
        public HttpResponse PostCancelFucProposal(string acr, int id)
        {
            return null;
        }

        [HttpCmd(HttpMethod.Get, "/fucs/{acr}/props/{id}/edit")]
        public HttpResponse GetEditFucProposal(string acr, int id)
        {
            return null;
        }
    }
}
