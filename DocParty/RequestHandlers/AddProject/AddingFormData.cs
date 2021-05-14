using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.AddProject
{
    public class AddingFormData
    {
        public string ProjectName { set; get; }
        public string Description { set; get; }
    }
}
