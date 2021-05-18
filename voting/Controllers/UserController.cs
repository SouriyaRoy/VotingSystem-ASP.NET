using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using voting.Models;

namespace voting.Controllers
{
    public class UserController : Controller
    {
        string BaseAddress = "https://localhost:44312/";

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "UserName, Password")] User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                Dictionary<string, string> form = new Dictionary<string, string>
               {
                   {"grant_type", "password"},
                   {"username", user.UserName},
                   {"password", user.Password},
               };

                var tokenResponse = client.PostAsync(client.BaseAddress + "token", new FormUrlEncodedContent(form)).Result;
                var token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                if (string.IsNullOrEmpty(token.Error))
                {
                    HttpCookie cookie = new HttpCookie("access_token");
                    cookie.Value = token.AccessToken;
                    Response.Cookies.Add(cookie);

                    // TO get user type
                    List<string> userType = new List<string>();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                    //client.BaseAddress = new Uri(BaseAddress);

                    var responseTask = client.GetAsync("User/GetUserType");
                    responseTask.Wait();

                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<List<string>>();
                        readTask.Wait();
                        userType = readTask.Result;
                    }
                    else
                    {
                        userType = Enumerable.Empty<string>().ToList();
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }

                    if (userType[0] == "0")
                    {
                        return RedirectToAction("GetAllPollsForUser", "Poll");
                    }
                    else if (userType[0] == "1")
                    {
                        return RedirectToAction("AdminHome", "Start");
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }
                    
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            user.UserType = 0;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                var postTask = client.PostAsJsonAsync<User>("User/AddNewUser", user);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error!!!!");

            return View(user);
        }

        public ActionResult Logout()
        {
            if (Request.Cookies["access_token"] != null)
            {
                Response.Cookies["access_token"].Expires = DateTime.Now.AddDays(-1);
                return RedirectToAction("Login");
            }
            return RedirectToAction("Login");
        }
    }
}
