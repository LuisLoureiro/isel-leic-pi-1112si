using System;
using mvc.Models.Entities;

namespace mvc.Models.Reposiroty
{
    public class RepositoryLocator
    {
        private static readonly CurricularUnitRepository UcRepo = new CurricularUnitRepository();
        private static readonly ProposalRepository PropRepo = new ProposalRepository();

        public static IRepository<K, V> Get<K, V>() where V : AbstractEntity<K>
        {
            if (typeof(V) == typeof(CurricularUnit))
                return UcRepo as IRepository<K, V>;
            if (typeof(V) == typeof(Proposal))
                return PropRepo as IRepository<K, V>;

            throw new InvalidOperationException("Os parâmetros de tipo indicados não foram reconhecidos.");
        }
    }
}