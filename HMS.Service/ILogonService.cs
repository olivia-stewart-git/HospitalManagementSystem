using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Service.ViewService.Controls;

namespace HMS.Service
{
    public interface ILogonService
    {
	    bool ExecuteLogin(int userId, string password);
	    void Logout();
    }
}
