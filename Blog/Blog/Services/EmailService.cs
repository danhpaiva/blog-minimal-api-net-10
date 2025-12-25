using System.Net.Mail;

namespace Blog.Services;

public class EmailService
{
    public bool Send(string toName,
        string toEmail,
        string subject,
        string body,
        string fromName = "Daniel Paiva",
        string fromEmail = "email@gmail.com") //aqui precisa ser um email valido
    {
        SmtpClient smtpClient = new(Configuration.Smtp.Host, Configuration.Smtp.Port);

        smtpClient.Credentials = new System.Net.NetworkCredential(Configuration.Smtp.Username, Configuration.Smtp.Password);
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;

        MailMessage mail = new();

        mail.From = new MailAddress(fromEmail, fromName);
        mail.To.Add(new MailAddress(toEmail, toName));
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        try
        {
            smtpClient.Send(mail);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
