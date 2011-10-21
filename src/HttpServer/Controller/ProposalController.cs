using PI.WebGarten;
using PI.WebGarten.MethodBasedCommands;

namespace HttpServer.Controller
{
    class ProposalController
    {
        [HttpCmd(HttpMethod.Get, "/props")]
        public HttpResponse GetFucProposal()
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
