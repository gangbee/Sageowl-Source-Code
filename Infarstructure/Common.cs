using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Infarstructure
{
    
    public class Common
    {
        public static string Encrypt(string password)
        {
            if (password == null) throw new ArgumentNullException("password");

            //encrypt data
            var data = Encoding.Unicode.GetBytes(password);
            

            //return as base64 string
            return Convert.ToBase64String(data);
        }

        public static bool SendEmail(string from,string to, string subject,  string body, string host, string password, int port, bool ssl)
        {
            
            //string host = "gmac13lu@sageowls.com";
            if (String.IsNullOrEmpty(from))
            {
                from = host;
              
            }

            var client = new SmtpClient() 
            {
                Host = host,
                UseDefaultCredentials = false,
                Port = port,
                Credentials = new NetworkCredential(from, password.Trim()),
                EnableSsl = ssl, // important
                DeliveryMethod = SmtpDeliveryMethod.Network,
                
                
            };
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(from, "Do_Not_Reply");
                //message.ReplyTo = from;
                message.To.Add(new MailAddress(to)); // My Gmail
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                client.Send(message);
                return true;

            }
            catch (Exception)
            {
                //throw ex;
            }

            return false;
        }


        public static bool SendDocumentsToInstitute(string from, string to, string subject, string body, string host, string password, int port, bool ssl, string filePath)
        {
            if (String.IsNullOrEmpty(from))
            {
                from = host;

            }

            var client = new SmtpClient()
            {
                Host = host,
                UseDefaultCredentials = false,
                Port = port,
                Credentials = new NetworkCredential(from, password.Trim()),
                EnableSsl = ssl, // important
                DeliveryMethod = SmtpDeliveryMethod.Network,


            };

            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(from, "Do_Not_Reply");
                message.Attachments.Add(new Attachment(filePath));
                //message.ReplyTo = from;
                message.To.Add(new MailAddress(to)); // My Gmail
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                client.Send(message);
                return true;

            }
            catch (Exception)
            {
                //throw ex;
            }

            return false;
        }
    }
}
