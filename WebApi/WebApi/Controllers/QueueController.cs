using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class QueueController : ApiController
    {
        // TODO pagination
        public IEnumerable<MessageEntity> Get()
        {
            return MvcApplication.Messages;
        }

        public void Step()
        {
            var message = MvcApplication.Messages.Where(x => x.State == EState.Ready).FirstOrDefault();

            if (message == null)
                return;

            message.State = EState.Processing;

            try
            {
                // Sending...
                message.ProcessedAt = DateTime.UtcNow;
                message.State = EState.Sent;
            }
            catch(Exception ex)
            {
                message.State = EState.Error;
                // Log error
            }            
        }
    }
}