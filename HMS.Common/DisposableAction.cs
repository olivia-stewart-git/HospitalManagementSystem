namespace HMS.Common;

public class DisposableAction : IDisposable
{
	readonly Action onDispose;

	public DisposableAction(Action onDispose)
	{
		this.onDispose = onDispose;
	}

	bool disposed = false;
	public void Dispose()
	{
		if (disposed) return;
		onDispose();
	}
}