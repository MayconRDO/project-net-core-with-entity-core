using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TasksAPI.Models;
using TasksAPI.Repositories.Interfaces;

namespace TasksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskController(ITaskRepository taskRepository, UserManager<ApplicationUser> userManager)
        {
            _taskRepository = taskRepository;
            _userManager = userManager;
        }

        public ActionResult Sync([FromBody]List<Task> tasks)
        {
            return Ok(_taskRepository.Sync(tasks));
        }

        public ActionResult Restor(DateTime dateTime)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            return Ok(_taskRepository.Restor(user, dateTime));
        }
    }
}