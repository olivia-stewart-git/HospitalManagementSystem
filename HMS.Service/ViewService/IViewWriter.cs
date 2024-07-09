namespace HMS.Service.ViewService;

public interface IViewWriter
{
	public void Clear();
	public void Write(RenderElement renderElement);
	void WriteLine(RenderElement renderElement);
    public void Write(string value);
	public void WriteLine(string value);
	public void WriteLine();
}