using System.Text;

namespace HMS.Service.ViewService.Controls;

/// <summary>
/// Data field that can provide a list of info about an object
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectDataField<T> : ViewControl
{
	readonly IEnumerable<DataFieldProperty<T>> properties;
	T? currentValue;

	public ObjectDataField(string name, params DataFieldProperty<T>[] properties) : this(name, (IEnumerable<DataFieldProperty<T>>)properties)
	{
	}

    public ObjectDataField(string name, IEnumerable<DataFieldProperty<T>> properties) : base(name)
	{
		this.properties = properties;
	}

	protected override List<RenderElement> OnRender()
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
		DoChange();
	}
}