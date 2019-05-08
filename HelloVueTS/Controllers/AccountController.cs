using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HelloVueTS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace HelloVueTS.Controllers
{
    public class AccountController : Controller
    {
        private readonly List<ApplicationUser> users = new List<ApplicationUser> {
                                                                    new ApplicationUser{Name = "hoge", Password = "1234"},
                                                                    new ApplicationUser{Name = "piyo", Password = "5678"}
                                                                };

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(ApplicationUser user, string returnUrl = null)
        {
            const string badUserNameOrPasswordMessage = "Username or password is incorrect.";
            if (user == null)
            {
                return BadRequest(badUserNameOrPasswordMessage);
            }

            // ユーザー名が一致するユーザーを抽出
            var lookupUser = users.FirstOrDefault(u => u.Name == user.Name);
            if (lookupUser == null)
            {
                return BadRequest(badUserNameOrPasswordMessage);
            }

            // パスワードの比較
            if (lookupUser?.Password != user.Password)
            {
                return BadRequest(badUserNameOrPasswordMessage);
            }

            // Cookies 認証スキームで新しい ClaimsIdentity を作成し、ユーザー名を追加します。
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, lookupUser.Name));

            // クッキー認証スキームと、上の数行で作成されたIDから作成された新しい ClaimsPrincipal を渡します。
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}