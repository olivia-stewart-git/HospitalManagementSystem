using System.Linq.Expressions;

namespace HMS.Service.ViewService.Controls;

/// <summary>
/// Returned property in a data field
/// </summary>
/// <typeparam name="T"></typeparam>
public class DataFieldProperty<T>
{
	public string Name { get; }
	readonly Expression<Func<T, object>> valueFunc;

	public DataFieldProperty(string name, Expression<Func<T, object>> valueFunc)
	{
		Name = name;
		this.valueFunc = valueFunc;
	}

	public string Evaluate(T targetObject)
	{
		var func = valueFunc.Compile();
		var returnValue = "Null Value";
		try
		{
			var result = func(targetObject);
			returnValue = result.ToString();
		}
		catch (Exception ex)
		{
			returnValue = $"Exception occurred retrieving value Message:{ex.Message}";
		}

		return returnValue;
	}
}