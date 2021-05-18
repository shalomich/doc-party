using DocParty.Models;
using DocParty.Services.Paginations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Paginators
{
    abstract class Paginator<Owner, Tenure> where Owner : IEntity
    {
        public int PageSize { set; get; }

        protected Dictionary<int, IPagination> _paginations = new Dictionary<int, IPagination>();
        protected int GetKey(Owner entity) => entity.Id;
        public abstract void Create(Owner owner);    
        public IPagination Get(Owner owner)
        {
            return _paginations[GetKey(owner)];
        }
        public abstract void MoveToNextPage(Owner owner);
        public abstract void MoveToPreviousPage(Owner owner);

    }
}
