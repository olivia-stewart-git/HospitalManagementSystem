namespace HMS.Service.ViewService;

public interface INavControl
{
	public bool IsHovered { get; set; }
	public void NavigateEnter();
	public void NavigateExit();
}