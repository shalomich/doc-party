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
    /// <summary>
    /// MVC controller is responsible for authentication and authorization. 
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Service for transfer responsibility from controllers
        /// to request or notification handlers.
        /// </summary>
        private readonly IMediator _mediator;
        
        public AccountController(IMediator mediator, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Redicrect to user profile page.
        /// </summary>
        
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

        /// <summary>
        /// Check for validity user login data
        /// and transfer responsibility of login  to need handler.
        /// </summary>
        /// <param name="loginRequest">Email and password</param>
        /// <returns>
        /// If login is successful then redirect to method Index
        /// else return login view with errors.
        /// </returns>
        
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromForm] LoginQuery loginRequest)
        {
            if (ModelState.IsValid)
            {
                // Getting object that contain errors about login user data.
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

        /// <summary>
        /// Check for validity user registration data
        /// and transfer responsibility of registration to need handler.
        /// </summary>
        /// <param name="loginRequest">Email, username and password</param>
        /// <returns>
        /// If registration is successful then redirect to user profile page
        /// else return registration view with errors.
        /// </returns>
        
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromForm] RegistrationCommand registerRequest)
        {
            if (ModelState.IsValid)
            {
                // Getting object that contain errors about login user data.
                ErrorResponce registerResponce = await _mediator.Send(registerRequest);

                if (ModelState.CheckErrors(registerResponce) == false)
                    return RedirectToRoute("user",new { userName = registerRequest.UserName });
            }
            return View(registerRequest);
        }

        /// <summary>
        /// Transfer responsibility of logout to need handler.
        /// </summary>
        /// <returns>
        /// Redirecct to login page.
        /// </returns>
        
        public async Task<IActionResult> LogoutAsync()
        {
            await _mediator.Send(new LogoutQuery());
            return RedirectToAction(nameof(Login));
        }

        /// <summary>
        /// Transfer responsibility of getting need auth properties
        /// from google oAuth provider.
        /// </summary>
        
        public async Task<IActionResult> GoogleConnect()
        {
            string redirectUrl = Url.Action(nameof(GoogleLogin), "Account");

            // Creation request for google getting auth properties handler.
            var request = new HandlerData<string, AuthenticationProperties>
            {
                Data = redirectUrl
            };

            var authProperties = await _mediator.Send(request);

            return new ChallengeResult("Google", authProperties);
        }

        /// <summary>
        /// Transfer responsibility of google login.
        /// </summary>
        /// <returns>
        /// If google login is successful then redirect to method Index
        /// else return login view with errors.
        /// </returns>
        
        public async Task<IActionResult> GoogleLogin()
        {
            var request = new HandlerData<Unit, ErrorResponce>
            {
                Data = Unit.Value
            };

            // Getting object that contain errors about login user data.
            ErrorResponce responce = await _mediator.Send(request);

            if (ModelState.CheckErrors(responce) == false)
                return RedirectToAction(nameof(Index));
            else 
                return RedirectToAction(nameof(Login));         
        }
    }
}
