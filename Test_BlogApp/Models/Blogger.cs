using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_BlogApp.Models
{
    public class Blogger
    {
        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public List<Post> Posts { get; set; }
    }
}
