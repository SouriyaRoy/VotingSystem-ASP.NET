using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using voting.Models;

namespace voting.Controllers
{
    public class CandidateController : Controller
    {
        string BaseAddress = "https://localhost:44312/";

        public ActionResult Index()
        {
            return RedirectToAction("GetAll");
        }

        public ActionResult GetAll()
        {
            string cookieValue = string.Empty;
            IEnumerable<Candidate> r = null;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                var responseTask = client.GetAsync("Candidate/GetAllCandidates");
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
            return View(r);
        }

        public ActionResult GetDetails(int id)
        {
            Candidate m = null;
            string cookieValue = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                var responseTask = client.GetAsync("Candidate/CandidateDetails/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Candidate>();
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
            string cookieValue = string.Empty;
            IEnumerable<Poll> r = null;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);
                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                var responseTask = client.GetAsync("Candidate/GetAllPolls");
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
            List<string> list = new List<string>();
            foreach (var item in r)
            {
                list.Add(item.PollName);
            }

            ViewBag.PollNames = r;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Candidate m)
        {
            using (HttpClient client = new HttpClient())
            {
                string cookieValue = string.Empty;
                client.BaseAddress = new Uri(BaseAddress);

                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                var postTask = client.PostAsJsonAsync<Candidate>("Candidate/AddNewCandidate", m);
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
            Candidate m = null;
            string cookieValue = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseAddress);

                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                var resposneTask = client.GetAsync("Candidate/CandidateDetails/" + id);
                resposneTask.Wait();

                var result = resposneTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Candidate>();
                    readTask.Wait();

                    m = readTask.Result;
                }
            }
            return View(m);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "CandidateId, Name, DOB, PollId")]Candidate m)
        {
            using (HttpClient client = new HttpClient())
            {
                string cookieValue = string.Empty;
                client.BaseAddress = new Uri(BaseAddress);

                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                var putTask = client.PutAsJsonAsync<Candidate>("Candidate/Editdetails", m);
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
            Candidate m = null;
            using (HttpClient client = new HttpClient())
            {
                string cookieValue = string.Empty;
                client.BaseAddress = new Uri(BaseAddress);

                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);

                var resposneTask = client.GetAsync("Candidate/CandidateDetails/" + id);
                resposneTask.Wait();

                var result = resposneTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Candidate>();
                    readTask.Wait();

                    m = readTask.Result;
                }
            }
            return View(m);
        }

        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteCandidate(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                string cookieValue = string.Empty;
                client.BaseAddress = new Uri(BaseAddress);
                if (Request.Cookies["access_token"] != null)
                {
                    cookieValue = Request.Cookies["access_token"].Value;
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieValue);
                var deleteTask = client.DeleteAsync("Candidate/DeleteCandidate/" + id);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}