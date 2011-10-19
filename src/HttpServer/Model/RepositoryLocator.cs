using System;
using System.Collections;
using System.Collections.Generic;

namespace HttpServer.Model
{
    class RepositoryLocator
    {
        private static readonly IDictionary<Type, IDictionary> _repo;

        static RepositoryLocator()
        {
            _repo = new Dictionary<Type, IDictionary>
                        {
                            {typeof (Proposal), new Dictionary<Int32, Proposal>()},
                            {typeof (CurricularUnit), new Dictionary<string, CurricularUnit>()}
                        };
        }

        public static IDictionary GetFor(Type t)
        {
            return _repo[t];
        }
    }
}
