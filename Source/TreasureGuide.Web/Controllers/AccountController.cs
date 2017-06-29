using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models;
using TreasureGuide.Web.Models.AccountModels;

namespace TreasureGuide.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _externalCookieScheme;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<IdentityCookieOptions> identityCookieOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetExternalLoginProviders()
        {
            var loginProviders = _signInManager.GetExternalAuthenticationSchemes();
            var results = loginProviders.Select(x => new
            {
                x.DisplayName,
                x.AuthenticationScheme
            }).ToList();
            return Ok(results);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AccessDenied()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var model = new ExternalLoginModel
            {
                Provider = provider,
                ReturnUrl = returnUrl
            };
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = model.ReturnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(model.Provider, redirectUrl);
            return Challenge(properties, model.Provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, remoteError);
            }
            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                return LocalRedirect(HttpHelper.CreateQuerystring("/#/error", new KeyValuePair<string, string>("message", "Something terrible went wrong! Please try again.")));
            }

            var parameters = new[]
            {
                new KeyValuePair<string, string>( "returnUrl", returnUrl),
                new KeyValuePair<string, string>( "token", externalLoginInfo.GetAccessToken())
            };

            // Sign in the user with this external login provider if the user already has a login.
            var loginResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, isPersistent: false);
            if (loginResult.Succeeded)
            {
                return LocalRedirect(HttpHelper.CreateQuerystring("/#/account/login", parameters));
            }
            if (loginResult.IsLockedOut)
            {
                return LocalRedirect(HttpHelper.CreateQuerystring("/#/error", new KeyValuePair<string, string>("message", "This account is not permitted to login.")));
            }
            else
            {
                var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser { UserName = email, Email = email };
                    var registerResult = await _userManager.CreateAsync(user);
                    if (registerResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        registerResult = await _userManager.AddLoginAsync(user, externalLoginInfo);
                        if (registerResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect("/");
                        }
                    }
                    AddErrors(registerResult);
                }
                return LocalRedirect(HttpHelper.CreateQuerystring("/#/account/login", parameters));
            }
        }

        [HttpGet]
        public IActionResult UserInfo()
        {
            var userModel = new UserInfoModel
            {
                UserName = User.FindFirstValue(ClaimTypes.Name),
                EmailAddress = User.FindFirstValue(ClaimTypes.Email),
                Roles = User.FindAll(ClaimTypes.Role).Select(x => x.Value)
            };
            return Ok(userModel);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
