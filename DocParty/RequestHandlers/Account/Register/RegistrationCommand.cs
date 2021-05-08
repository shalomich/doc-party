using DocParty.RequestHandlers;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.Account.Register
{
    public class RegistrationCommand : IRequest<ErrorResponce>
    {
        
        [Required(ErrorMessage = "No username specified")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "No email specified")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "No password specified")]
        public string Password { get; set; }
    }
}
