using System.Net;
using System.Net.Mail;

namespace HMS.Service;

/// <summary>
/// Simple implementation of smtp
/// Errors are caught as I do not want to make each DB entity have correct email
/// </summary>
public class MailService : IMailService
{
	const string Email = "hms@gmail.com";

	public void SendEmail(string receiver, string subject, string content)
	{
		var smtpClient = new SmtpClient("smtp.gmail.com")
		{
			Port = 587,
			Credentials = new NetworkCredential("hmsusername", "!hmspass"),
			EnableSsl = true,
		};

		try
		{
			smtpClient.Send(Email, receiver, "subject", "body");
		}
		catch
		{
		}
	}
}