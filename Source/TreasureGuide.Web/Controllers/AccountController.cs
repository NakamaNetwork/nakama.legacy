using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TreasureGuide.Web.Helpers;
using TreasureGuide.Web.Models;
using TreasureGuide.Web.Models.AccountModels;
using TreasureGuide.Web.Models.AccountViewModels;

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
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return Ok("Register");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                return Ok(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return Unauthorized();
            }
            else
            {
                var collection = new Dictionary<string, string>
                {
                    {"loginProvider",info.LoginProvider },
                    {"emailAddress", info.Principal.FindFirstValue(ClaimTypes.Email)},
                    {"userName", info.Principal.FindFirstValue(ClaimTypes.Name)}
                };
                return Redirect(HttpHelper.CreateQuerystring(collection, "/#/account/register"));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.EmailAddress };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var info = await _signInManager.GetExternalLoginInfoAsync();
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return UserInfo();
                    }
                }
                AddErrors(result);
            }
            return BadRequest(ModelState);
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
