using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers
{
    public class ErrorResponce
    {
        public IEnumerable<string> Errors { set; get; }

        public ErrorResponce(IEnumerable<string> errors)
        {
            Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }
    }
}
