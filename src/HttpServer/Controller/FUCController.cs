using System.Net;
using HttpServer.Model.Entities;
using HttpServer.Model.Repository;
using HttpServer.Views;
using PI.WebGarten;
using PI.WebGarten.MethodBasedCommands;
using System;

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
            return new HttpResponse(HttpStatusCode.OK, new FucsView(_repo.GetAll(typeof(CurricularUnit))));
        }

        [HttpCmd(HttpMethod.Get, "/fucs/{acr}")]
        public HttpResponse GetFuc(string acr)
        {
            CurricularUnit fuc;

            try
            {
                fuc = _repo.GetById(typeof(CurricularUnit), acr);
            }catch(Exception)
            {
                return new HttpResponse(HttpStatusCode.NotFound);
            }
            
            return new HttpResponse(HttpStatusCode.OK, new FucsView(fuc));
        }

        [HttpCmd(HttpMethod.Get, "/fucs/new")]
        public HttpResponse GetCreateFuc()
        {
            return null;
        }

        [HttpCmd(HttpMethod.Get, "/fucs/{acr}/edit")]
        public HttpResponse GetEditFucForm(string acr)
        {
            return null;
        }

        [HttpCmd(HttpMethod.Post, "/fucs/{acr}/edit")]
        public HttpResponse PostEditFucForm(string acr)
        {
            return null;
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
