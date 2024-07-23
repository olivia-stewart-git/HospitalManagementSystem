namespace HMS.Service;

public interface IMailService
{
	void SendEmail(string receiver, string subject, string content);
}