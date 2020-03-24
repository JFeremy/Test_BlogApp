using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_BlogApp.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int IdBlogger { get; set; }
        public string Title { get; set;}
        public string Content { get; set; }
    }
}
