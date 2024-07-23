namespace HMS.Service.ViewService;

public interface IBindable
{
	void BindProperty(Action<object> factoryBinding, string propertyName);
}