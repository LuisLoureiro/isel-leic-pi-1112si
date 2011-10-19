using System;
using System.Collections.Generic;
using HttpServer.Model.Entities;
using HttpServer.Model.Mappers;

namespace HttpServer.Model.Repository
{
    public class CurricularUnitRepository : IRepository<string, CurricularUnit>
    {
        private readonly IDictionary<Type, IMapper<string, CurricularUnit>> _mappers = new Dictionary<Type, IMapper<string, CurricularUnit>>();

        public IEnumerable<CurricularUnit> GetAll(Type type)
        {
            return _mappers[type].GetAll();
        }

        public CurricularUnit GetById(Type type, string key)
        {
            return _mappers[type].GetById(key);
        }

        public void Insert(IEnumerable<CurricularUnit> values)
        {
            // Verificar se existe mapper para o tipo de dados do enumerável.
            IMapper<string, CurricularUnit> mapper;
            if (!_mappers.TryGetValue(typeof(CurricularUnit), out mapper))
                throw new InvalidOperationException(string.Format(
                    "Não existe nenhum mapper para o tipo indicado({0}).", typeof(CurricularUnit)));

            foreach (CurricularUnit value in values)
            {
                mapper.Insert(value);
            }
        }

        public void Insert(CurricularUnit value)
        {
            // Verificar se existe mapper para o tipo de dados do enumerável.
            IMapper<string, CurricularUnit> mapper;
            if (!_mappers.TryGetValue(typeof(CurricularUnit), out mapper))
                throw new InvalidOperationException(string.Format(
                    "Não existe nenhum mapper para o tipo indicado({0}).", typeof(CurricularUnit)));

            mapper.Insert(value);
        }

        public void Add(Type type, IMapper<string, CurricularUnit> mapper)
        {
            // Se já existir um Mapper para este tipo retorna excepção;
            IMapper<string, CurricularUnit> exists;
            if (_mappers.TryGetValue(type, out exists))
                throw new InvalidOperationException(string.Format(
                    "Não é possível substituir o mapper existente para o tipo indicado({0}).", type));

            _mappers[type] = mapper;
        }
    }
}
