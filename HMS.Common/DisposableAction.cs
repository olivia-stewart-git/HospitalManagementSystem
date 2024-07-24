namespace HMS.Common;

/// <summary>
/// Invokes action on dispose
/// </summary>
public class DisposableAction : IDisposable
{
	readonly Action onDispose;

	public DisposableAction(Action onDispose)
	{
		this.onDispose = onDispose;
	}

	bool disposed;
	public void Dispose()
	{
		if (disposed) return;
		disposed = true;
        onDispose();
	}
}