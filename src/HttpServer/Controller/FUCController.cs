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
        public HttpResponse Get()
        {
            return new HttpResponse(HttpStatusCode.OK, new FucsView(_repo.GetAll(typeof(CurricularUnit))));
        }

        [HttpCmd(HttpMethod.Get, "/fucs/{acr}")]
        public HttpResponse Get(string acr)
        {
            CurricularUnit fuc;
            try
            {
                fuc = _repo.GetById(typeof (CurricularUnit), acr);
            }catch(Exception)
            {
                return new HttpResponse(HttpStatusCode.BadRequest);
            }
            
            return new HttpResponse(HttpStatusCode.OK, new FucsView(fuc));
        }

        //[HttpCmd(HttpMethod.Get, "/fucs/{acr}/edit")]
        //public HttpResponse Get(string acr)
        //{
            
        //}

        [HttpCmd(HttpMethod.Post, "/fucs/{acr}/edit")]
        public HttpResponse Post(string acr)
        {
            return null;
        }
    }
}
