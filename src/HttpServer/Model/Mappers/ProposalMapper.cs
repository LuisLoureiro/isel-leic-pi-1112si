using System;
using System.Collections.Generic;
using HttpServer.Model.Entities;

namespace HttpServer.Model.Mappers
{
    public class ProposalMapper : IMapper<UInt32, Proposal>
    {
        private readonly IDictionary<UInt32, Proposal> _props = new Dictionary<uint, Proposal>();

        public IEnumerable<Proposal> GetAll()
        {
            return _props.Values;
        }

        public Proposal GetById(UInt32 key)
        {
            return _props[key];
        }

        public void Insert(Proposal value)
        {
            // Se já existir um valor para o mesmo identificador envia excepção.
            if (_props.ContainsKey(value.Key))
                throw new InvalidOperationException(string.Format(
                    "Já existe uma proposta com o mesmo identificador({0}).", value.Key));

            _props[value.Key] = value;
        }
    }
}