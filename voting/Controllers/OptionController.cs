using Newtonsoft.Json;
using PollingSystemMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using voting.Models;

namespace voting.Controllers
{
    public class OptionController : Controller
    {
        string BaseAddress = "https://localhost:44312/";

        [HttpGet]
        public ActionResult IncreaseCount(int CandidateId, int PollId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                string cookieValue = string.Empty;
                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                var putTask = client.GetAsync(string.Format("Option/IncreaseCount/{0}/{1}", CandidateId, PollId));
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAllPollsForUser", "Poll", new { message = "Voted" });
                }
            }
            return RedirectToAction("Index");
        }
    }
}