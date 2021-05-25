using DocParty.Models;
using DocParty.RequestHandlers;
using DocParty.Services;
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
        private readonly AuthorAssignService _assignService;

        public RegisterHandler(UserManager<User> userManager, SignInManager<User> signInManager, AuthorAssignService assignService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _assignService = assignService ?? throw new ArgumentNullException(nameof(assignService));
        }

        public async Task<ErrorResponce> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            var user = new User { Email = request.Email, UserName = request.UserName};

            var result = await _userManager.CreateAsync(user, request.Password);
            
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                await _assignService.CheckRegisteredUser(user.Email);
            }
            else
            {
                errors = result.Errors.Select(error => error.Description).ToList();
            }

            return new ErrorResponce(errors);       
        }
    }
}
