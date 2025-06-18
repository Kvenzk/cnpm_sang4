using cnpm_sang4.Data;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace cnpm_sang4.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password, bool remember)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Điền đầy đủ username và password";
                return View();
            }

            if (ContainsUnicode(username))
            {
                ViewBag.Error = "username không được dùng kí tự unicode";
                return View();
            }

            if (ContainsUnicode(password))
            {
                ViewBag.Error = "password không được dùng kí tự unicode";
                return View();
            }

            var users = UserData.GetUsers();
            if (!users.ContainsKey(username) || users[username].Password != password)
            {
                ViewBag.Error = "sai thông tin đăng nhập";
                return View();
            }

            HttpContext.Session.SetString("username", username);

            if (remember)
                Response.Cookies.Append("remember_user", username, new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) });

            return RedirectToAction("Home");
        }

        public IActionResult Home()
        {
            var user = HttpContext.Session.GetString("username") ?? Request.Cookies["remember_user"];
            if (user == null)
                return RedirectToAction("Login");

            ViewBag.Username = user;
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("remember_user");
            return RedirectToAction("Login");
        }

        private bool ContainsUnicode(string input)
        {
            return Regex.IsMatch(input, @"[^\u0000-\u007F]+");
        }
    }

}
