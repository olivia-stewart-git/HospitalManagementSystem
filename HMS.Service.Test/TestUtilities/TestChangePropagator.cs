using HMS.Common;

namespace HMS.Service.TestUtilities;

public class TestChangePropagator : IChangePropagator<IEnumerable<TestModel>>
{
	List<TestModel> value;

	public string Name { get; init; } = string.Empty;
	public List<TestModel> Values
	{
		get => value;
		set
		{
			this.value = value;
			RegisterChanged();
		}
	}

	public EventHandler<IEnumerable<TestModel>>? OnChange { get; set; }
	public void RegisterChanged()
	{
		OnChange?.Invoke(this, Values);
	}
}