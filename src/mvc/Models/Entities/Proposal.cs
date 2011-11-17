using System;

namespace mvc.Models.Entities
{
    public class Proposal : AbstractEntity<long>
    {
        public string Owner { get; private set; }
        public Status State { get; private set; }

        private static long _id = 0; //Começará no 1

        public Proposal(CurricularUnit info, string owner) : base(++_id)
        {
            Info = info;
            Owner = owner;
            State = Status.Pending;
        }

        public Proposal() : base(0)
        {
        }

        public CurricularUnit Info { get; set; }

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