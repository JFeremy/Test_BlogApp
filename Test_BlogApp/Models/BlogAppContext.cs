using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Test_BlogApp.Models
{
    public class BlogAppContext : DbContext
    {
        public BlogAppContext(DbContextOptions<BlogAppContext> options)
            : base(options)
        {
        }

        public DbSet<Blogger> Bloggers { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
