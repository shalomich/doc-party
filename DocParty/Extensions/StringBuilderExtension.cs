using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocParty.Extensions
{
    public static class StringBuilderExtension
    {
        public static StringBuilder GetRussianSymbols(this StringBuilder builder)
        {
            int russianAlphabetLength = 32;
            for (int i = 0; i < russianAlphabetLength; i++)
            {
                builder.Append((char)('а' + i));
                builder.Append((char)('А' + i));
            }

            return builder;
        }
    }
}
