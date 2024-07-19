using HMS.Data.Models;

namespace HMS.Service;

public class Environment : IEnvironment
{
	public UserModel? CurrentUser { get; set; }
	public SystemRole CurrentRole { get; set; } = SystemRole.None;
}