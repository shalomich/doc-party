using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers
{
    public class SnapshotFormData
    {
        public string Name { set; get; }
        public string Description { set; get; }
    }
}
