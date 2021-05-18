using Newtonsoft.Json;
using voting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using voting.Models;
//using votingSystem.ViewModels;

namespace caseApp.Controllers
{
    public class PollController : Controller
    {
        string BaseAddress = "https://localhost:44312/";

        public ActionResult Index()
        {
            return RedirectToAction("GetAll");
        }

        public ActionResult GetAll()
        {
            IEnumerable<Poll> r = null;
            string cookieValue = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);
                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                var responseTask = client.GetAsync("Poll/GetAllPolls");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Poll>>();
                    readTask.Wait();
                    r = readTask.Result;
                }
                else
                {
                    r = Enumerable.Empty<Poll>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(r);
        }

        public ActionResult Details(int id)
        {
            Poll m = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);
                string cookieValue = string.Empty;
                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                var responseTask = client.GetAsync("Poll/GetPollDetails/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Poll>();
                    readTask.Wait();
                    m = readTask.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(m);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Poll m)
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

                var postTask = client.PostAsJsonAsync<Poll>("Poll/AddNewPoll", m);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error!!!!");

            return View(m);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Poll m = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                string cookieValue = string.Empty;
                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                var resposneTask = client.GetAsync("Poll/GetPollDetails/" + id);
                resposneTask.Wait();

                var result = resposneTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Poll>();
                    readTask.Wait();

                    m = readTask.Result;
                }
            }
            return View(m);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "PollId, PollName, PollStatus, Result")] Poll m)
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

                var putTask = client.PutAsJsonAsync<Poll>("Poll/EditPolldetails", m);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(m);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Poll m = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                string cookieValue = string.Empty;
                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                var resposneTask = client.GetAsync("Poll/GetPollDetails/" + id);
                resposneTask.Wait();

                var result = resposneTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Poll>();
                    readTask.Wait();

                    m = readTask.Result;
                }
            }
            return View(m);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePoll(int id)
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

                var deleteTask = client.DeleteAsync("Poll/DeletePoll/" + id);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult GetOptions(int id)
        {
            IEnumerable<Option> r = null;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                string cookieValue = string.Empty;
                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                var responseTask = client.GetAsync("Poll/GetOptionsForPoll/" + id);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Option>>();
                    readTask.Wait();
                    r = readTask.Result;
                }
                else
                {
                    r = Enumerable.Empty<Option>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPollsForUser(string message = "")
        {
            if (!String.IsNullOrEmpty(message))
            {
                ViewBag.Message = "Voted Successfully!!";
            }
            string result = string.Empty;
            string cookieValue = string.Empty;
            List<PollDetailsViewModel> pollDetailsViewModels = new List<PollDetailsViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                client.DefaultRequestHeaders.Clear();

                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("Poll/GetAllPollOptions");
                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    pollDetailsViewModels = JsonConvert.DeserializeObject<List<PollDetailsViewModel>>(EmpResponse);

                }
                return View(pollDetailsViewModels);
            }
        }

        public ActionResult ClosePoll(int id)
        {
            IEnumerable<Candidate> r = null;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                string cookieValue = string.Empty;
                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                var responseTask = client.GetAsync("Poll/ClosePoll/" + id);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Candidate>>();
                    readTask.Wait();
                    r = readTask.Result;
                }
                else
                {
                    r = Enumerable.Empty<Candidate>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return RedirectToAction("GetAll");
        }

        public ActionResult GetAllActive()
        { 
            IEnumerable<Poll> r = null;


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44312/");


                var responseTask = client.GetAsync("Poll/GetActivePolls");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                { 
                    var readTask = result.Content.ReadAsAsync<IList<Poll>>();
                    readTask.Wait();
                    r = readTask.Result;
                }
                else
                {
                    r = Enumerable.Empty<Poll>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(r);
        }

        [HttpGet]
        public ActionResult GetResultForParticularPoll(int? pollId)
        {
            PollResultViewModel pollResultViewModel = new PollResultViewModel();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);
                string cookieValue = string.Empty;
                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                var responseTask = client.GetAsync("Poll/GetOptionsForPoll/" + pollId);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<PollResultViewModel>();
                    readTask.Wait();
                    pollResultViewModel = readTask.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(pollResultViewModel);
        }
    }
}