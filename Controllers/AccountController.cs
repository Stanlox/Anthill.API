using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Anthill.API.Models;
using Anthill.API.Services;
using Anthill.API.ViewModels;
using Anthill.Infastructure.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Anthill.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly EmailService service;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, EmailService service)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.service = service;
        }

        [HttpGet("[action]")]
        public IActionResult Login()
        {
            return Ok();
        }

        [HttpPost("[action]")]

        public async Task<IActionResult> Login([FromBody] LoginViewModel details)
        {
            if (ModelState.IsValid)
            {
                User user = await this.userManager.FindByEmailAsync(details.Email);

                if (user == null)
                {
                    ModelState.AddModelError(nameof(details.Email), $"'There is no user this {details.Email} address'");
                    return BadRequest(ModelState);
                }
                else
                {
                    SignInResult result = await this.signInManager.PasswordSignInAsync(user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        await Authenticate(details.Email);
                        return Ok();
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(LoginViewModel.Password), "Invalid password");
                        return BadRequest(ModelState);
                    }
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.Name,
                    Email = model.Email
                };

                IdentityResult result = await this.userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        return BadRequest(ModelState);
                    }
                }

                await GenerateEmailRegisterConfirmation(user, model);
                return Ok();

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                {
                    return StatusCode(403);
                }

                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                service.SendEmail(email, "Сброс пароля", $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>link</a>");
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code, string password)
        {
            var user = await userManager.FindByIdAsync(userId);
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                await Authenticate(user.Email);
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        private async Task Authenticate(string email)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, email)
                };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        private async Task GenerateEmailRegisterConfirmation(User user, RegisterViewModel model)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = Url.Action(
                       "ConfirmEmail",
                       "Account",
                       new { userId = user.Id, code = code, password = model.Password },
                       protocol: HttpContext.Request.Scheme);

            service.SendEmail(model.Email, "Подтверждения регистрации", $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");
        }
    }
}
