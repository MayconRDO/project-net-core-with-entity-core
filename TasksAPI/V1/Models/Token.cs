using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasksAPI.API.Models
{
    public class Token
    {
        [Key]
        public int Id { get; set; }
        public string RefreshToken { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public bool Used { get; set; }
        public DateTime ExpirationToken { get; set; }
        public DateTime ExpirationRefreshToken { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModifield { get; set; }
    }
}
