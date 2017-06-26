using CommonModels;
using System;
using System.Collections.Generic;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class SendController : ApiController
    {
        [HttpPost]
        public void Message(Message message)
        {
            var entity = new MessageEntity
            {
                Channel = message.Channel,
                Body = message.Body,
                CreatedAt = DateTime.UtcNow,
                State = EState.Ready
            };

            MvcApplication.Messages.Add(entity);
        }
    }
}