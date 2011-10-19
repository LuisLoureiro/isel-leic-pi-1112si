using System;
using System.Collections.Generic;
using HttpServer.Model.Entities;

namespace HttpServer.Model.Mappers
{
    public class CurricularUnitMapper : IMapper<string, CurricularUnit>
    {
        private readonly IDictionary<string, CurricularUnit> _fucs = new Dictionary<string, CurricularUnit>();

        public IEnumerable<CurricularUnit> GetAll()
        {
            return _fucs.Values;
        }

        public CurricularUnit GetById(string key)
        {
            return _fucs[key];
        }

        public void Insert(CurricularUnit value)
        {
            // Se já existir um valor para a mesma chave envia excepção.
            if (_fucs.ContainsKey(value.Key))
                throw new InvalidOperationException(string.Format(
                    "Já existe uma unidade curricular com a mesma chave({0}).", value.Key));

            _fucs[value.Key] = value;
        }
    }
}