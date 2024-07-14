namespace HMS.Service.ViewService.Controls;

public class ObjectDataField<T> : ViewControl
{
	T? currentValue;

	public ObjectDataField(string? name = null) : base(name ?? typeof(T).Name + "-objectDataField")
	{
	}

	public override List<RenderElement> Render()
	{
		if (currentValue == null)
		{
			return [RenderElement.Default($"No data found for {typeof(T).Name}")];
		}
		return RenderElement.Empty();
	}

	public void Set(T value)
	{
		currentValue = value;

    }
}