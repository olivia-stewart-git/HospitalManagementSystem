using HMS.Service.Interaction;

namespace HMS.Service;

public class LogonService : ILogonService
{
	readonly IInputService inputService;

	public LogonService(IInputService inputService)
	{
		this.inputService = inputService;
	}

	public void StartLogonProcess()
	{

	}
}