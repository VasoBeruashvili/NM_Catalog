using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Routing;

namespace NM_Catalog.Helpers
{
    public static class EmailHelper
    {
        public static bool SendEmail(string Subject, string SmtpHost, int SmtpPort, bool EnableSsl, string SenderAddress, string login, string password, List<string> BCC, string MessageText)
        {
            using (MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress(SenderAddress),
                Priority = MailPriority.High,
                Body = MessageText,
                Subject = Subject
            })
            {
                try
                {
                    SmtpClient smtpClient = new SmtpClient()
                    {
                        Host = SmtpHost,
                        Port = SmtpPort,
                        Credentials = new NetworkCredential(login, password),
                        EnableSsl = EnableSsl
                    };

                    BCC.ForEach(bcc => { mailMessage.Bcc.Add(bcc); });

                    smtpClient.Send(mailMessage);
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }
    }
}