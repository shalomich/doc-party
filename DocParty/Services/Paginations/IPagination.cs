using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Paginations
{
    interface IPagination
    {
        public int PageSize { set; get; }
        public IQueryable<T> GetPage<T>(IQueryable<T> objects);
    }
}
