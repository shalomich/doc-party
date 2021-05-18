using DocParty.Models;
using DocParty.Services.Paginations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Paginators
{
    class NumberedPaginator<Owner, Tenure> : Paginator<Owner, Tenure> where Owner : IEntity
    {
        public override void Create(Owner owner)
        {
            _paginations.Add(GetKey(owner), new NumberedPagination { PageSize = PageSize });
        }

        public override void MoveToNextPage(Owner owner)
        {
            var pagination = (NumberedPagination)Get(owner);
            pagination.PageNumber++;
        }

        public override void MoveToPreviousPage(Owner owner)
        {
            var pagination = (NumberedPagination)Get(owner);
            pagination.PageNumber--;
        }
    }
}
