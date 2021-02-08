using System;
using System.Collections.Generic;
using TasksAPI.Models;

namespace TasksAPI.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        List<Task> Sync(List<Task> tasks);
        List<Task> Restor(ApplicationUser user, DateTime dateLastSync);
    }
}
