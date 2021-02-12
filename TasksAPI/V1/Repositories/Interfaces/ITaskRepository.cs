using System;
using System.Collections.Generic;
using TasksAPI.API.Models;

namespace TasksAPI.API.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        List<Task> Sync(List<Task> tasks);
        List<Task> Restor(ApplicationUser user, DateTime dateLastSync);
    }
}
