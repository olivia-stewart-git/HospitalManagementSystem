using System.Linq.Expressions;

namespace HMS.Service.ViewService.Controls;

public class TableViewColumn<T>
{
	public Expression<Func<T, object>> ValueFunc { get; }
	public string? OverrideName { get; }

	public TableViewColumn(Expression<Func<T, object>> valueFunc, string? overrideName = null) 
	{
		ValueFunc = valueFunc;
		OverrideName = overrideName;
	}
}