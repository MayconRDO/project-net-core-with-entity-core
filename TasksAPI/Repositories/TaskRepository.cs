using System;
using System.Collections.Generic;
using System.Linq;
using TasksAPI.DataBase;
using TasksAPI.Models;
using TasksAPI.Repositories.Interfaces;

namespace TasksAPI.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TasksContext _context;

        public TaskRepository(TasksContext context)
        {
            _context = context;
        }

        public List<Task> Restor(ApplicationUser user, DateTime dateLastSync)
        {
            var query = _context.Tasks.Where(t => t.UserId == user.Id).AsQueryable();

            if (dateLastSync == null)
            {
                query.Where(t => t.DateCreated >= dateLastSync || t.DateModifield >= dateLastSync);
            }

            return query.ToList<Task>();
        }

        public List<Task> Sync(List<Task> tasks)
        {
            var newTasks = tasks.Where(t => t.Id == 0);

            foreach (var task in tasks)
            {
                _context.Add(task);
            }

            var updateTasks = tasks.Where(t => t.Id != 0);

            foreach (var task in updateTasks)
            {
                _context.Tasks.Update(task);
            }

            _context.SaveChanges();

            return newTasks.ToList();

        }

    }
}
