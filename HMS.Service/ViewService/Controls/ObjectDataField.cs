using System.Text;
using HMS.Common;

namespace HMS.Service.ViewService.Controls;

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
		DoChange();
	}
}