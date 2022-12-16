using System;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace VeterinaryClinic.Data.Entities
{
    public static class EMail
    {
        public async static Task SendMessage(string mailto, string caption, string message)
        {
            try
            {
                string from = "comand_project@mail.ru";
                string password = "Pg6KeqtHA597Zt6Y5Gpt";

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption; // Тема сообщения
                mail.Body = message;
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.mail.ru";
                client.Port = 25;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from.Split('@')[0], password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                await Task.Run(() => client.Send(mail));
                // client.Dispose();
                await Task.Run(() => mail.Dispose());
            }
            catch (Exception e)
            {
                int i = 0;
                i++;
            }
        }
    }
}
