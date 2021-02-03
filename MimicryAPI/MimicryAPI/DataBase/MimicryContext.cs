using Microsoft.EntityFrameworkCore;
using MimicryAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicryAPI.DataBase
{
    public class MimicryContext : DbContext
    {
        public MimicryContext(DbContextOptions<MimicryContext> options) : base(options)
        {
        }

        public DbSet<Word> Words { get; set; }

    }
}
