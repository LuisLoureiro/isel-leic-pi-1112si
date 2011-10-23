using System;
namespace HttpServer.Model.Entities
{
    public class Proposal : AbstractEntity<long>
    {
        private readonly CurricularUnit _info;
        public string Owner { get; private set; }
        private Status _status;

        private static long _id = 0; //Começará no 1

        public Proposal(CurricularUnit info, string owner) : base(++_id)
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