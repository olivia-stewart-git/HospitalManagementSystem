namespace HMS.Service;

public class Environment : IEnvironment
{
	public Guid? CurrentUser { get; set; }
	public SystemRole CurrentRole { get; set; } = SystemRole.None;
}