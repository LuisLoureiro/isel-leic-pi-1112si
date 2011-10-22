using System;
namespace HttpServer.Model.Entities
{
    // TODO falta adicionar identificação do utilizador autenticado que deu origem a esta proposta
    public class Proposal : AbstractEntity<long>
    {
        private readonly CurricularUnit _info;

        private static long _id = 0; //Começará no 1

        public Proposal(CurricularUnit info) : base(++_id)
        {
            _info = info;
        }

        public CurricularUnit Info
        {
            get { return _info; }
        }
    }
}