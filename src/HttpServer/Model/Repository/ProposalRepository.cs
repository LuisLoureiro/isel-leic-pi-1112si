using System;
using System.Collections.Generic;
using HttpServer.Model.Entities;
using HttpServer.Model.Mappers;

namespace HttpServer.Model.Repository
{
    public class ProposalRepository : IRepository<UInt32, Proposal>
    {
        private readonly IDictionary<Type, IMapper<UInt32, Proposal>> _mappers = new Dictionary<Type, IMapper<UInt32, Proposal>>();

        public IEnumerable<Proposal> GetAll(Type type)
        {
            return _mappers[type].GetAll();
        }

        public Proposal GetById(Type type, UInt32 key)
        {
            return _mappers[type].GetById(key);
        }

        public void Insert(IEnumerable<Proposal> values)
        {
            // Verificar se existe mapper para o tipo de dados do enumerável.
            IMapper<UInt32, Proposal> mapper;
            if (!_mappers.TryGetValue(typeof(Proposal), out mapper))
                throw new InvalidOperationException(string.Format(
                    "Não existe nenhum mapper para o tipo indicado({0}).", typeof(CurricularUnit)));

            foreach (Proposal value in values)
            {
                mapper.Insert(value);
            }
        }

        public void Insert(Proposal value)
        {
            // Verificar se existe mapper para o tipo de dados do enumerável.
            IMapper<UInt32, Proposal> mapper;
            if (!_mappers.TryGetValue(typeof(Proposal), out mapper))
                throw new InvalidOperationException(string.Format(
                    "Não existe nenhum mapper para o tipo indicado({0}).", typeof(Proposal)));

            mapper.Insert(value);
        }

        public void Add(Type type, IMapper<UInt32, Proposal> mapper)
        {
            // Se já existir um Mapper para este tipo retorna excepção;
            IMapper<UInt32, Proposal> exists;
            if (_mappers.TryGetValue(type, out exists))
                throw new InvalidOperationException(string.Format(
                    "Não é possível substituir o mapper existente para o tipo indicado({0}).", type));

            _mappers[type] = mapper;
        }
    }
}
