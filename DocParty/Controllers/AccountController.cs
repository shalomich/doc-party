using DocParty.RequestHandlers.Account.Login;
using DocParty.RequestHandlers.Account.Register;
using DocParty.RequestHandlers;
using DocParty.RequestHandlers.Logout;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DocParty.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using DocParty.Extensions;

namespace DocParty.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        
        public AccountController(IMediator mediator, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Authorize]
        public IActionResult Index()
        {
            string name = User.Identity.Name;
            return RedirectToRoute("user", new { userName = User.Identity.Name});
        }
 
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromForm] LoginQuery loginRequest)
        {
            if (ModelState.IsValid)
            {
                ErrorResponce loginResponce = await _mediator.Send(loginRequest);
                if (ModelState.CheckErrors(loginResponce) == false)
                    return RedirectToAction(nameof(Index));
            }

            return View(loginRequest);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromForm] RegistrationCommand registerRequest)
        {
            if (ModelState.IsValid)
            {
                ErrorResponce registerResponce = await _mediator.Send(registerRequest);

                if (ModelState.CheckErrors(registerResponce) == false)
                    return RedirectToRoute("user",new { userName = registerRequest.UserName });
            }
            return View(registerRequest);
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await _mediator.Send(new LogoutQuery());
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> GoogleConnect()
        {
            string redirectUrl = Url.Action(nameof(GoogleLogin), "Account");

            var request = new HandlerData<string, AuthenticationProperties>
            {
                Data = redirectUrl
            };

            var authProperties = await _mediator.Send(request);

            return new ChallengeResult("Google", authProperties);
        }

        public async Task<IActionResult> GoogleLogin()
        {
            var request = new HandlerData<Unit, ErrorResponce>
            {
                Data = Unit.Value
            };

            ErrorResponce responce = await _mediator.Send(request);

            if (ModelState.CheckErrors(responce) == false)
                return RedirectToAction(nameof(Index));
            else 
                return RedirectToAction(nameof(Login));         
        }
    }
}
