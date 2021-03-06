﻿
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;
using WCDApi.DataBase.Entity;

namespace WCDApi.Mail
{
    public class MailSender
    {
        private readonly MailSettings _mailSettings;
        public MailSender(MailSettings mailSettings)
        {
            _mailSettings = mailSettings;
        }

        public async Task<Boolean> sendMail(String address, String password)
        {
            try
            {
                // Send the message 
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_mailSettings.Mail));
                message.To.Add(new MailboxAddress(address));
                message.Subject = "Welcome! Your Website has changed";

                message.Body = new TextPart("plain")
                {
                  Text = @"Hey

                    Your website has changed ! Check this out: " + password + @"

                    -- WCDapp"
                };
                
                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(_mailSettings.SMTPServer, _mailSettings.Port, true);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Pass);

                    await client.SendAsync(message);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not send e-mail. Exception caught: " + e);
                return false;
            }


        }
        public async Task<Boolean> sendAllert(String address, String url)
        {
            try
            {
                // Send the message 
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_mailSettings.Mail));
                message.To.Add(new MailboxAddress(address));
                message.Subject = "Welcome! Your Website has changed";

                message.Body = new TextPart("plain")
                {
                    Text = @"Hey

                    Your website has changed ! Check this out: " + url + @"

                    -- WCDapp"
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(_mailSettings.SMTPServer, _mailSettings.Port, true);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Pass);

                    await client.SendAsync(message);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not send e-mail. Exception caught: " + e);
                return false;
            }


        }
    }
}


