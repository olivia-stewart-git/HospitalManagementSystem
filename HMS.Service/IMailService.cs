namespace HMS.Service;

public interface IMailService
{
	bool TrySendEmail(string receiver, string subject, string content);
}