using DocParty.Models;
using DocParty.Services.Paginations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Paginators
{
    class IncreasedPaginator<Owner, Tenure> : Paginator<Owner, Tenure> where Owner : IEntity
    {
        public override void Create(Owner owner)
        {
            _paginations.Add(GetKey(owner), new IncreasedPagination { PageSize = PageSize });
        }

        public override void MoveToNextPage(Owner owner)
        {
            var pagination = (IncreasedPagination)Get(owner);
            pagination.Increase++;
        }

        public override void MoveToPreviousPage(Owner owner)
        {
            var pagination = (IncreasedPagination)Get(owner);
            pagination.Increase++;
        }
    }
}
