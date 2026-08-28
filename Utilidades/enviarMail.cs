using System;
using System.Net;
using System.Net.Mail;

namespace Utilidades
{
    public static class enviarMail
    {
        public static void sendMail(string to, string asunto, string body, string from, string passwordAplicacion, string displayName = "Recuperacion de Cuenta")
        {
            try
            {
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(from, displayName)
                };
                mail.To.Add(to);

                mail.Subject = asunto;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(from, passwordAplicacion),
                    EnableSsl = true
                };
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR AL ENVIAR EL MENSAJE: " + ex.Message, ex);
            }
        }
    }
}
