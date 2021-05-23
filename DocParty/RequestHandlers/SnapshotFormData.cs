using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers
{
    public class SnapshotFormData
    {
        [Required]
        public string Name { set; get; }

        [Required]
        public string Description { set; get; }
    }
}
