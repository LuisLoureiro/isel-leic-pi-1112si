using System;
using System.Collections.Generic;

namespace HttpServer.Model
{
    class ProposalRepository : IRepository<Int32, Proposal>
    {
        private readonly IDictionary<Int32, Proposal> _repo = new Dictionary<Int32, Proposal>();

        public IEnumerable<Proposal> GetAll()
        {
            return _repo.Values;
        }

        public Proposal GetById(int key)
        {
            return _repo[key];
        }
    }
}
