﻿using System;
using System.Collections.Generic;
using System.Linq;
using TasksAPI.DataBase;
using TasksAPI.API.Models;
using TasksAPI.API.Repositories.Interfaces;

namespace TasksAPI.API.Repositories
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
            var newTasks = tasks.Where(t => t.Id == 0).ToList();
            var updateTasks = tasks.Where(t => t.Id != 0).ToList();

            foreach (var task in tasks)
            {
                _context.Add(task);
            }            

            foreach (var task in updateTasks)
            {
                _context.Tasks.Update(task);
            }

            _context.SaveChanges();

            return newTasks.ToList();

        }

    }
}
