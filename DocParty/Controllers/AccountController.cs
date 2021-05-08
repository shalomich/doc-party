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

namespace DocParty.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        public IActionResult Index()
        {
            return Content($"{User.Identity.Name} is authorized");
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

                if (loginResponce.Errors.Any() == false)
                    return RedirectToAction(nameof(Index));
                else
                {
                    foreach (string message in loginResponce.Errors)
                        ModelState.AddModelError("", message);
                }
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

                if (registerResponce.Errors.Any() == false)
                    return RedirectToAction(nameof(Index));
                else
                {
                    foreach (string message in registerResponce.Errors)
                        ModelState.AddModelError("", message);
                }
            }
            return View(registerRequest);
        }

        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            await _mediator.Send(new LogoutQuery());
            return RedirectToAction(nameof(Index));
        }
    }
}
