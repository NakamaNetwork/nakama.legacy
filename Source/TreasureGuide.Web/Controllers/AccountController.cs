using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TreasureGuide.Web.Models;
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

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
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
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var model = new ExternalLoginConfirmationViewModel { Email = email };
                return Ok(model);
            }
        }
    }
}
