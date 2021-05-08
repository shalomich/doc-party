using DocParty.Handlers.Account.Login;
using DocParty.Handlers.Account.Register;
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
            return Content($"Need to authorize");
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginQuery request)
        {
            return await AuthRequestHandle(request);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationCommand request)
        {
            return await AuthRequestHandle(request);
        }

        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            await _mediator.Send(new LogoutQuery());
            return RedirectToAction(nameof(Index));
        }

        private async Task<IActionResult> AuthRequestHandle(IRequest<ErrorResponce> request)
        {
            if (ModelState.IsValid)
            {
                ErrorResponce responce = await _mediator.Send(request);

                if (responce.Errors.Any() == false)
                    return RedirectToAction(nameof(Index));
                else
                {
                    foreach (string message in responce.Errors)
                        ModelState.AddModelError("", message);
                }
            }

            return new JsonResult(ModelState);
        }
    }
}
