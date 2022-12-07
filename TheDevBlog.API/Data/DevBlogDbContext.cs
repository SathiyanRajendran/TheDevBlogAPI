using Microsoft.EntityFrameworkCore;
using TheDevBlog.API.Models.Entities;

namespace TheDevBlog.API.Data
{
    public class DevBlogDbContext:DbContext
    {
        public DevBlogDbContext(DbContextOptions options):base(options)
        {

        }

        //DBSet
        public DbSet<Post> Posts { get; set; }
    }
}
