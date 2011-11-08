using System;
namespace HttpServer.Model.Entities
{
    public class Proposal : AbstractEntity<long>
    {
        private readonly CurricularUnit _info;
        public string Owner { get; private set; }
        public Status State { get; private set; }

        private static long _id = 0; //Começará no 1

        public Proposal(CurricularUnit info, string owner) : base(++_id)
        {
            _info = info;
            Owner = owner;
            State = Status.Pending;
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
            if (!State.Equals(Status.Pending))
                throw new InvalidOperationException("Não é possivel actualizar uma proposta que esteja no estado de aceite ou cancelada.");

            if (State.Equals(newStatus))
                throw new InvalidOperationException("Não é possível actualizar uma proposta para um estado igual ao actual.");

            State = newStatus;
        }
    }
}