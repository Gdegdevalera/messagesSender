using log4net;
using SharedModels;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using WebApi.DAL;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    public class QueueController : Controller
    {
        private readonly Persistance _persistance = new Persistance();
        private readonly ILog logger = LogManager.GetLogger(typeof(QueueController));

        public ActionResult Get()
        {
            var paginagtion = Request.Headers.GetPagination();

            if (paginagtion.Page < 1
                || paginagtion.Capacity > 500
                || paginagtion.Capacity < 1)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var total = _persistance.Messages.Count();
            var query = _persistance.Messages
                .OrderBy(x => x.Id)
                .Skip(paginagtion.Skip)
                .Take(paginagtion.Capacity);

            Response.Headers.SetPagination(paginagtion, total);

            return Json(query.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public async Task<HttpStatusCodeResult> Step()
        {
            var message = _persistance.Messages.Where(x => x.State == EState.Ready).FirstOrDefault();

            if (message == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            message.State = EState.Processing;
            _persistance.SaveChanges();

            try
            {
                await Send(message);

                logger.Info(string.Format("Message id:{0} '{1}' was sent via {2} successfully",
                    message.Id,
                    message.Body.SafeSubstring(0, 100),
                    message.Channel));

                message.ProcessedAt = DateTime.UtcNow;
                message.State = EState.Sent;
                _persistance.SaveChanges();
            }
            catch (Exception ex)
            {                
                logger.Error(string.Format("Error with sending message id:{0} '{1}' via {2} occured. {3}. {4}. {5}",
                    message.Id,
                    message.Body.SafeSubstring(0, 100),
                    message.Channel,
                    ex.GetType(),
                    ex.Message,
                    ex.StackTrace));

                try
                {
                    message.State = EState.Error;
                    _persistance.SaveChanges();
                }
                catch(Exception saveChangesException)
                {
                    logger.Error(string.Format("Error with marking message id:{0} as 'Error' occured. {3}. {4}. {5}",
                        message.Id,
                        message.Body.SafeSubstring(0, 100),
                        message.Channel,
                        saveChangesException.GetType(),
                        saveChangesException.Message,
                        saveChangesException.StackTrace));
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        
        private async Task Send(MessageEntity message)
        {
            var recipientEmail = WebConfigurationManager.AppSettings["send:TargetEmail"];
            using (var mail = new MailMessage())
            {
                mail.To.Add(recipientEmail);
                mail.Body = string.Format("Тип сообщения: {0}{1}Сообщение: {2}",
                    message.Channel, Environment.NewLine, message.Body);
                mail.Subject = "Новое сообщение";

                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(mail);
                }
            }
        }
    }
}