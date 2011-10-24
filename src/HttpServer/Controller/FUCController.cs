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
        public HttpResponse GetFucs()
        {
            return new HttpResponse(HttpStatusCode.OK, new FucsView(_repo.GetAll()));
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

        [HttpCmd(HttpMethod.Get, "/fucs/{acr}/edit")]
        public HttpResponse GetEditFucForm(string acr)
        {
            var fuc = _repo.GetById(acr);

            return new HttpResponse(HttpStatusCode.OK, new ProposalView(fuc, ResolveUri.ForEdit(fuc)));
        }

        [HttpCmd(HttpMethod.Post, "/fucs/{acr}/edit")]
        public HttpResponse PostEditFucForm(string acr, IEnumerable<KeyValuePair<string, string>> content, IPrincipal principal)
        {
            float ects;
            if (!float.TryParse(content.Where(p => p.Key.Equals("ects")).FirstOrDefault().Value, out ects))
                throw new ArgumentException("Valor de ECTS inválido");

            // Criar uma unidade curricular, associada à proposta, com os valores recolhidos do formulário de edição.
            var uc = new CurricularUnit(content.Where(p => p.Key.Equals("name")).FirstOrDefault().Value,
                                        content.Where(p => p.Key.Equals("acr")).FirstOrDefault().Value,
                                        content.Where(p => p.Key.Equals("tipoObrig")).FirstOrDefault().Value.Equals("obrigatoria"),
                                        TranslateSemester(content),
                                        ects)
                         {
                             Results = content.Where(p => p.Key.Equals("results")).FirstOrDefault().Value,
                             Objectives = content.Where(p => p.Key.Equals("objectives")).FirstOrDefault().Value,
                             Assessment = content.Where(p => p.Key.Equals("assessment")).FirstOrDefault().Value,
                             Program = content.Where(p => p.Key.Equals("program")).FirstOrDefault().Value
                         };
            
            // Actualizar as precedências
            //uc.UpdatePrecedences(content); 

            // Criar a proposta de alteração de unidade curricular.
            var prop = new Proposal(uc, principal.Identity.Name);
            RepositoryLocator.Get<long, Proposal>().Insert(prop);

            return new HttpResponse(HttpStatusCode.SeeOther).WithHeader("Location", ResolveUri.For(prop));
        }

        private static ushort TranslateSemester(IEnumerable<KeyValuePair<string, string>> content)
        {
            ushort val = 0x00;

            for(int i = 0; i<10; i++)
                if (content.Where(k => k.Key.Equals(i.ToString())).FirstOrDefault().Value == i.ToString())
                    val = (ushort)(val | (0x01 << i));

            return val;
        }
    }
}
