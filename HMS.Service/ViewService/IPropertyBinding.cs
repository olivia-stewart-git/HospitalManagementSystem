namespace HMS.Service.ViewService;

/// <summary>
/// Describes object that allows binding to its changeable properties
/// </summary>
public interface IPropertyBinding
{
	void BindProperty(Action<object> factoryBinding, string propertyName);
	void BindProperty<T>(Action<T> factoryBinding, string propertyName);
}