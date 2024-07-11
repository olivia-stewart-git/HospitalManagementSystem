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
			DoChange();
		}
	}

	public EventHandler<IEnumerable<TestModel>>? OnChange { get; set; }
	public void DoChange()
	{
		OnChange?.Invoke(this, Values);
	}
}