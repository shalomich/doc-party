using DocParty.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.AccountHandlers.GoogleConnect
{
    class GoogleConnectHandler : IRequestHandler<HandlerData<string, AuthenticationProperties>,AuthenticationProperties>
    {
        private SignInManager<User> SignInManager { get; }

        public GoogleConnectHandler(SignInManager<User> signInManager)
        {
            SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public Task<AuthenticationProperties> Handle(HandlerData<string, AuthenticationProperties> request, CancellationToken cancellationToken)
        {
            string redirectUrl = request.Data;
            return Task.FromResult(SignInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl));
        }
    }
}
