using DocParty.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.Logout
{
    class LogoutHandler : IRequestHandler<LogoutQuery>
    {
        private readonly SignInManager<User> _signInManager;

        public LogoutHandler(SignInManager<User> signInManager)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        /// <summary>
        /// Sign out from account.
        /// </summary>
        /// <returns>Nothing.</returns>
        public async Task<Unit> Handle(LogoutQuery request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            return Unit.Value;
        }
    }
}
