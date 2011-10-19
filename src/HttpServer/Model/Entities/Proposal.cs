using System;
namespace HttpServer.Model.Entities
{
    // TODO falta adicionar identificação do utilizador autenticado que deu origem a esta proposta
    public class Proposal : AbstractEntity<UInt32>
    {
        private readonly CurricularUnit _info;

        public Proposal(UInt32 id, CurricularUnit info) : base(id)
        {
            _info = info;
        }

        public CurricularUnit Info
        {
            get { return _info; }
        }
    }
}