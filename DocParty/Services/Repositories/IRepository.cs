using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Repositories
{
    public interface IRepository<T,K>
    {
        T Select(K id);
        void Create(K id,T obj);
        void Update(K id,T obj);
        void Delete(K id);
    }
}
