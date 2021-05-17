using DocParty.Models;
using DocParty.RequestHandlers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.Account.Register
{
    class RegisterHandler : IRequestHandler<RegistrationCommand, ErrorResponce>
    {
        
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegisterHandler(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<ErrorResponce> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            var user = new User { Email = request.Email, UserName = request.UserName};

            var result = await _userManager.CreateAsync(user, request.Password);
            
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }
            else
            {
                errors = result.Errors.Select(error => error.Description).ToList();
            }

            return new ErrorResponce(errors);       
        }
    }
}
