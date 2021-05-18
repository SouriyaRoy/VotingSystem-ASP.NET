using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using votingSystem.ViewModels;

namespace voting.Controllers
{
    public class StartController : Controller
    {
        string Baseurl = "https://localhost:44312/";

        public ActionResult AdminHome()
        {
            ViewBag.Title = "AdminHome";

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetClosedPollsForAdmin(int id)
        {
            IEnumerable<PollResultViewModel> pollDetailsViewModels = Enumerable.Empty<PollResultViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();

                string cookieValue = string.Empty;
                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("Poll/GetAllClosedPolls");

                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    pollDetailsViewModels = JsonConvert.DeserializeObject<IEnumerable<PollResultViewModel>>(EmpResponse);

                }
                return View(pollDetailsViewModels);
            }
        }
    }
}