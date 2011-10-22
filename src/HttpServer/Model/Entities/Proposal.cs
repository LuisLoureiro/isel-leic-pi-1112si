using System;
namespace HttpServer.Model.Entities
{
    public class Proposal : AbstractEntity<UInt32>
    {
        private readonly CurricularUnit _info;
        public User Owner { get; private set; }
        private Status _status;

        public Proposal(UInt32 id, CurricularUnit info, User owner) : base(id)
        {
            _info = info;
            Owner = owner;
            _status = Status.Pending;
        }

        public CurricularUnit Info
        {
            get { return _info; }
        }

        public void UpdateStatus(Status newStatus)
        {
            // Só pode ser feita uma actualização sobre uma proposta que esteja no estado pendente.
            // Se o novo estado for igual ao anterior, envia excepção.
            // Permite encontrar erros de programação.
            if (!_status.Equals(Status.Pending))
                throw new InvalidOperationException("Não é possivel actualizar uma proposta que esteja no estado de aceite ou cancelada.");

            if (_status.Equals(newStatus))
                throw new InvalidOperationException("Não é possível actualizar uma proposta para um estado igual ao actual.");

            _status = newStatus;
        }
    }
}