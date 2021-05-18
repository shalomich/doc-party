using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Paginations
{
    class NumberedPagination : IPagination
    {
        private const int MinPageSize = 1;
        private const int MinPageNumber = 1;
        private const int MaxPageSize = int.MaxValue;

        private int _pageNumber = MinPageNumber;
        private int _pageSize = MaxPageSize;
        public int PageSize
        {
            set
            {
                if (value < MinPageSize)
                {
                    _pageSize = MaxPageSize;
                }
                else _pageSize = value;
            }
            get
            {
                return _pageSize;
            }
        }

        public int PageNumber
        {
            set
            {
                if (value < MinPageNumber)
                {
                    _pageNumber = MinPageNumber;
                }
                else _pageNumber = value;
            }
            get
            {
                return _pageNumber;
            }
        }

        public int CalculatePageCount<T>(IQueryable<T> query) => (int)Math.Ceiling(query.Count() / (double)PageSize);

        public IQueryable<T> GetPage<T>(IQueryable<T> objects)
        {
            int pageCount = CalculatePageCount(objects);
            bool hasNextPage = PageNumber < pageCount;
            bool hasPreviousPage = PageNumber > 1;

            int pageNumber = PageNumber;
            if (PageNumber > pageCount)
            {
                pageNumber = pageCount;
            }

            return objects.Skip(PageSize * (pageNumber - 1)).Take(PageSize);

        }        
    }
}
