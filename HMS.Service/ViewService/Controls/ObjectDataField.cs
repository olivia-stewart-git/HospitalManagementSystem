using System.Text;

namespace HMS.Service.ViewService.Controls;

public class ObjectDataField<T> : ViewControl
{
	readonly IEnumerable<DataFieldProperty<T>> properties;
	T? currentValue;

	public ObjectDataField(params DataFieldProperty<T>[] properties) : this((IEnumerable<DataFieldProperty<T>>)properties)
	{
	}

    public ObjectDataField(IEnumerable<DataFieldProperty<T>> properties) : base(typeof(T).Name + "-objectDataField")
	{
		this.properties = properties;
	}

	public override List<RenderElement> Render()
	{
		if (currentValue == null)
		{
			return [RenderElement.Default($"No data found for {typeof(T).Name}")];
		}

		var sb = new StringBuilder();
		foreach (var property in properties)
		{
			var retrievedValue = property.Evaluate(currentValue);
			sb.AppendLine($"{property.Name}: {retrievedValue}");
		}

		return [RenderElement.Default(sb.ToString())];
	}

	public void Set(T value)
	{
		currentValue = value;
    }
}