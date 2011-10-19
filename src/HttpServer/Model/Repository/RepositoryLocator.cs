using System;
using System.Collections;
using System.Collections.Generic;
using HttpServer.Model.Entities;

namespace HttpServer.Model.Repository
{
    public class RepositoryLocator
    {
        private static readonly IRepository _repo = new Repository<object, AbstractEntity<object>>();

        public static IRepository<K, V> Get<K, V>() where V : AbstractEntity<K>
        {
            return _repo as IRepository<K, V>;
        }
    }
}