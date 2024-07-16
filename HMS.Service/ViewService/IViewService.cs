namespace HMS.Service.ViewService;

public interface IViewService
{
	View? CurrentView { get; }

	T SwitchView<T>() where T : View;
	void Redraw();
	void LoadLastView();
	void WriteApplicationError(Exception ex);
	void ClearError();
}