using DocParty.Models;
using DocParty.RequestHandlers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.Account.Login
{
    class LoginHandler : IRequestHandler<LoginQuery, ErrorResponce>
    {
        private const string InvalidEmailMessage = "This email is not registered";
        private const string InvalidPasswordMessage = "This password is wrong for current email";

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public LoginHandler(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public async Task<ErrorResponce> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

			User user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                errors.Add(InvalidEmailMessage);
                return new ErrorResponce(errors);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }
            else errors.Add(InvalidPasswordMessage);
            
            return new ErrorResponce(errors);
		}
	}
}
