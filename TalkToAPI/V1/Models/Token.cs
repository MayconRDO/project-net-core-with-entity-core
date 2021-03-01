using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalkToAPI.V1.Models;

namespace TalkToAPI.V1.Models
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
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
