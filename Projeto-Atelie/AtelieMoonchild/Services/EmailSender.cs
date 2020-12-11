using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AtelieMoonchild.Services
{
    public class EmailSender
    {
        // Se for Gmail - Ativar conta appmenosseguro-https://myaccount.google.com/u/1/lesssecureapps?pli=1&rapt=AEjHL4PNwKiMIGwTttY6i0mz8ACKnFE8SgG04BOuzhu0B1u1vH4DP8o9d64MvF6wO4jEyPm1UJevy9O0ezBBv02oM7mSVnMp0A

        public async Task<bool> Mail(string To, string From, string Sub, string Mensagem)
        {
            var m = new MailMessage()
            {
                Subject = Sub,
                Body = Mensagem,
                IsBodyHtml = true
            };
            MailAddress to = new MailAddress(To);
            m.To.Add(to);
            m.From = new MailAddress(From);
            m.Sender = to;
            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com", // smtp.mail.yahoo.com // smtp.live.com
                Port = 587,
                Credentials = new NetworkCredential("e-mail", "senha"),
                EnableSsl = true
            };
            try
            {
                await smtp.SendMailAsync(m);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
