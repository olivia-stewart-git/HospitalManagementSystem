namespace HMS.Service.ViewService;

/// <summary>
/// This is an interface for controls in which they can be navigated to across the page
/// </summary>
public interface INavControl
{
	public bool IsHovered { get; set; }
	public void NavigateEnter();
	public void NavigateExit();
}