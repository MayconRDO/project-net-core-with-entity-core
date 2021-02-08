using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksAPI.Models;

namespace TasksAPI.Repositories.Interfaces
{
    public interface IApplicationUserRepository
    {
        ApplicationUser Get(string email, string password);
        void Add(ApplicationUser user, string password);
    }
}
