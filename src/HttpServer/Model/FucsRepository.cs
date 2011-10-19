using System.Collections.Generic;

namespace HttpServer.Model
{
    class FucsRepository : IRepository<string, CurricularUnit>
    {
        private readonly IDictionary<string, CurricularUnit> _repo = new Dictionary<string, CurricularUnit>();

        public IEnumerable<CurricularUnit> GetAll()
        {
            return _repo.Values;
        }

        public CurricularUnit GetById(string key)
        {
            return _repo[key];
        }
    }
}
