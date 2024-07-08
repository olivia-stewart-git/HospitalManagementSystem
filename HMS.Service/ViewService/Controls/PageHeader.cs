namespace HMS.Service.ViewService.Controls;

public class PageHeader : ViewControl
{
	readonly string title;
	readonly string subtitle;

	public PageHeader(string title, string subtitle)
	{
		this.title = title;
		this.subtitle = subtitle;
	}

	public override string Render()
	{
		return title + subtitle;
	}
}