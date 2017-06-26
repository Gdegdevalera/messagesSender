using Client.Extensions;
using Client.Models;
using Newtonsoft.Json;
using RestSharp;
using SharedModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _webApiUrl = WebConfigurationManager.AppSettings["WebApiUrl"];

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Queue()
        {
            return View();
        }
        
        public async Task<ActionResult> FetchQueue(int capacity, int page = 1)
        {
            var request = new RestRequest();
            request.Method = Method.GET;
            request.SetPagination(page, capacity);

            var client = new RestClient(_webApiUrl + "/queue/get");
            var response = await client.ExecuteGetTaskAsync(request);
            if(response.StatusCode != HttpStatusCode.OK)
            {
                return new HttpStatusCodeResult(response.StatusCode);
            }

            var messages = JsonConvert.DeserializeObject<List<MessageEntity>>(response.Content);
            var pagination = response.Headers.GetPagination();

            var model = new QueueModel
            {
                Messages = messages,
                Page = pagination.Page,
                Capacity = pagination.Capacity,
                TotalPages = pagination.TotalPages
            };

            return PartialView(model);
        }
        
        [HttpPost]
        public async Task<ActionResult> Send(Message message)
        {
            if(message.Body == null || message.Body.Length > 4000)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var request = new RestRequest();
            request.Method = Method.POST;
            request.AddJsonBody(message);

            var client = new RestClient(_webApiUrl + "/send/message");
            var response = await client.ExecutePostTaskAsync(request);

            return new HttpStatusCodeResult(response.StatusCode);
        }

        public async Task<HttpStatusCodeResult> QueueStep()
        {
            var request = new RestRequest();
            request.Method = Method.GET;

            var client = new RestClient(_webApiUrl + "/queue/step");
            var response = await client.ExecutePostTaskAsync(request);

            return new HttpStatusCodeResult(response.StatusCode);
        }
    }
}