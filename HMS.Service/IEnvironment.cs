using HMS.Data.Models;

namespace HMS.Service;

public interface IEnvironment
{
	UserModel CurrentUser { get; set; }
	SystemRole CurrentRole { get; set; }
	void Exit();
}