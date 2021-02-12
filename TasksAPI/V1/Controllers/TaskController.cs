using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TasksAPI.API.Models;
using TasksAPI.API.Repositories.Interfaces;

namespace TasksAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskController(ITaskRepository taskRepository, UserManager<ApplicationUser> userManager)
        {
            _taskRepository = taskRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Sincronizar tarefas
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("sync")]
        public ActionResult Sync([FromBody]List<Task> tasks)
        {
            return Ok(_taskRepository.Sync(tasks));
        }

        /// <summary>
        /// Restaurar tarefas
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("restor")]
        public ActionResult Restor(DateTime dateTime)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            return Ok(_taskRepository.Restor(user, dateTime));
        }

        /// <summary>
        /// Visualizar modelo de tarefas
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("model")]
        public ActionResult Model()
        {
            return Ok(new Models.Task());
        }
    }
}