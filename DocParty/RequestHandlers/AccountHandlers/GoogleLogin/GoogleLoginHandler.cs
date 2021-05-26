using DocParty.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.AccountHandlers.GoogleLogin
{
    class GoogleLoginHandler : IRequestHandler<HandlerData<Unit, ErrorResponce>, ErrorResponce>
    {
        private const string GoogleConnectionErrorMessage = "Can not connect to google account";
        private SignInManager<User> SignInManager { get; }
        private UserManager<User> UserManager { get; }

        public GoogleLoginHandler(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <summary>
        /// If user is registered then login
        /// else create new account.
        /// </summary>
        /// <param name="request">Nothing.</param>
        /// <returns>
        /// If user is registered or registration is successful then empty error responce
        /// else concrete error.
        /// </returns>
        public async Task<ErrorResponce> Handle(HandlerData<Unit, ErrorResponce> request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            ExternalLoginInfo info = await SignInManager.GetExternalLoginInfoAsync();
            
            if (info == null)
            {
                errors.Add(GoogleConnectionErrorMessage);
                return new ErrorResponce(errors);
            }
               
            var signInResult = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            
            string userName = info.Principal.FindFirst(ClaimTypes.Name).Value.Replace(" ", "_");
            string email = info.Principal.FindFirst(ClaimTypes.Email).Value;
            string[] userInfo = { userName, email };

            if (signInResult.Succeeded)
                return new ErrorResponce(errors);
            else
            {
                var user = new User
                {
                    Email = email,
                    UserName = userName
                };

                IdentityResult identityResult = await UserManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    identityResult = await UserManager.AddLoginAsync(user, info);
                    if (identityResult.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, false);
                    }
                }
                
                errors = identityResult.Errors.Select(error => error.Description).ToList();

                return new ErrorResponce(errors);

            }
        }
    }
}
