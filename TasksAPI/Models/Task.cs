using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasksAPI.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateHour { get; set; }
        public string Local { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool Completed { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModifield { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
