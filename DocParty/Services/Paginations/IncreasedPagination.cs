using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Paginations
{
    class IncreasedPagination : IPagination
    {
        private const int MinIncrease = 1;
        private const int MinPageSize = 1;
        private const int MaxPageSize = int.MaxValue;

        private int _increase = MinIncrease;
        private int _pageSize = MaxPageSize;

        public int Increase 
        {
            set 
            {
                if (value < MinIncrease)
                    _increase = MinIncrease;
            }
            get 
            {
                return _increase;
            } 
        }
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
        private int PageIncrease => PageSize * Increase;
        public IQueryable<T> GetPage<T>(IQueryable<T> objects)
        {
            if (PageIncrease > objects.Count())
                Increase = (int)Math.Ceiling(objects.Count() / (double)PageSize);
            
            return objects.Take(PageIncrease);
        }
    }
}
