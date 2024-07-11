namespace HMS.Service;

public interface IEnvironment
{
	Guid? CurrentUser { get; set; }
	SystemRole CurrentRole { get; set; }
}