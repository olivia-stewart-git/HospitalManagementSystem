namespace HMS.Common;

/// <summary>
/// Implementation of observer pattern to have entity watch for changes
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IChangePropagator<T>
{
    public EventHandler<T> OnChange { get; set; }
    public void DoChange();
}

/// <summary>
/// I would have like to have changed properties on controls to use this if I had time
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObservableProperty<T> : IChangePropagator<T>
{
	public ObservableProperty()
	{
		_value = default(T);
	}

	public ObservableProperty(T? value)
	{
		_value = value;
	}

	T? _value;
	public T? Value
	{
		get => _value;
		set
		{
			var lastValue = _value;
			_value = value;
			if (!lastValue.Equals(_value))
			{
				DoChange();
			}
        }
	}

    public EventHandler<T> OnChange { get; set; }
    public void DoChange()
    {
        OnChange?.Invoke(this, _value);
    }
}