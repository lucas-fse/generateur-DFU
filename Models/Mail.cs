using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace GenerateurDFUSafir.Models
{
    public class Mail
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string AttachementPath { get; set; }

        public void btnSendMail()
        {
            try
            {
                string from = From.Trim();
                string to = To.Trim();
                string subject = Subject;
                string message = Message;

                SmtpClient objSmtpClient = new SmtpClient();
                objSmtpClient.Host = "smtp.laposte.net";
                objSmtpClient.Credentials = new System.Net.NetworkCredential("iisProd.Conductix", "Conductix=1IIS");
                objSmtpClient.Port = 587;
                objSmtpClient.EnableSsl = true;
                //objSmtpClient.Send(from, to, subject, message);
                MailMessage mailMessage = new MailMessage(from, to);
                if (!string.IsNullOrWhiteSpace(AttachementPath))
                {
                    Attachment PJ = new Attachment(AttachementPath);
                    mailMessage.Attachments.Add(PJ);
                }
                mailMessage.Subject = subject;
                mailMessage.Body = message;

                objSmtpClient.Send(mailMessage);
            }
            catch (SmtpException ex) 
            {
            }
        }
    }
}