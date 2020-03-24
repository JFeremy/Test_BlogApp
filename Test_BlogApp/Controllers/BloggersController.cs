using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Test_BlogApp.Models;

namespace Test_BlogApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BloggersController : ControllerBase
    {
        private readonly BlogAppContext _context;

        public BloggersController(BlogAppContext context)
        {
            _context = context;
        }

        // GET: Bloggers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blogger>>> GetBloggers()
        {
            if(_context.Bloggers.ToList().Count == 0)
            {
                using (StreamReader r = new StreamReader("example.json"))
                {
                    string json = r.ReadToEnd();
                    List<BloggerImport> items = JsonConvert.DeserializeObject<List<BloggerImport>>(json);
                    items.ForEach(item =>
                    {
                        Blogger newBlogger = new Blogger
                        {
                            Firstname = item.Firstname,
                            Lastname = item.Lastname,
                            Posts = new List<Post>()
                        };
                        _context.Bloggers.Add(newBlogger);

                        item.Posts.ForEach(post =>
                        {
                            Post newPost = new Post
                            {
                                Title = post.Title,
                                Content = post.Content,
                                IdBlogger = newBlogger.Id
                            };

                            _context.Posts.Add(newPost);
                            _context.SaveChanges();
                        });

                        _context.SaveChanges();

                    });
                    await _context.SaveChangesAsync();
                }
            }

            List<Blogger> bloggers = await _context.Bloggers.ToListAsync();
            List<Post> posts = await _context.Posts.ToListAsync<Post>();
            bloggers.ForEach(blogger =>
            {
                blogger.Posts = posts.FindAll(post => post.IdBlogger == blogger.Id);
            });
            return bloggers;
        }

        // GET: Bloggers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Blogger>> GetBlogger(long id)
        {
            var blogger = await _context.Bloggers.FindAsync(id);

            if (blogger == null)
            {
                return NotFound();
            }

            return blogger;
        }

        // PUT: Bloggers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlogger(int id, Blogger blogger)
        {
            if (id != blogger.Id)
            {
                return BadRequest();
            }

            _context.Entry(blogger).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BloggerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: Bloggers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Blogger>> PostBlogger(Blogger blogger)
        {
            _context.Bloggers.Add(blogger);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlogger", new { id = blogger.Id }, blogger);
        }

        // DELETE: Bloggers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Blogger>> DeleteBlogger(int id)
        {
            var blogger = await _context.Bloggers.FindAsync(id);
            if (blogger == null)
            {
                return NotFound();
            }

            _context.Bloggers.Remove(blogger);
            await _context.SaveChangesAsync();

            return blogger;
        }

        private bool BloggerExists(int id)
        {
            return _context.Bloggers.Any(e => e.Id == id);
        }
    }
}
