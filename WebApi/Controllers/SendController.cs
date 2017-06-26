using SharedModels;
using System;
using System.Net;
using System.Web.Mvc;
using WebApi.DAL;

namespace WebApi.Controllers
{
    public class SendController : Controller
    {
        private readonly Persistance _persistance = new Persistance();

        [HttpPost]
        public HttpStatusCodeResult Message(Message message)
        {
            var entity = new MessageEntity
            {
                Channel = message.Channel,
                Body = message.Body,
                CreatedAt = DateTime.UtcNow,
                State = EState.Ready
            };

            _persistance.Messages.Add(entity);
            _persistance.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}