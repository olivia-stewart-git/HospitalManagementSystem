public class HospitalManagementSystem
{
	static HospitalManagementSystem? Instance { get; set; }

	readonly CancellationTokenSource cancellationTokenSource;

	public HospitalManagementSystem(CancellationTokenSource cancellationTokenSource)
	{
		this.cancellationTokenSource = cancellationTokenSource;
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void Exit()
	{
		cancellationTokenSource.Cancel();
	}

	public static void QuitApplication()
	{
		Instance?.Exit();
	}
}