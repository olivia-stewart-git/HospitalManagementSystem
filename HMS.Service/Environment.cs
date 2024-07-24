using HMS.Data.Models;

namespace HMS.Service;

/// <summary>
/// Environment that persists for app lifetime
/// </summary>
public class Environment : IEnvironment
{
	public UserModel? CurrentUser { get; set; }
	public SystemRole CurrentRole { get; set; } = SystemRole.None;
}