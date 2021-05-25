using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Email
{
    public class EmailMessage
    {
        public string Subject { set; get; }
        public string Text { set; get; }
    }
}
