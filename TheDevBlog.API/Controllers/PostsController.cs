using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheDevBlog.API.Data;
using TheDevBlog.API.Models.DTO;
using TheDevBlog.API.Models.Entities;

namespace TheDevBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly DevBlogDbContext dbContext;

        public PostsController(DevBlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult>GetAllPosts()
        {
            var posts=await dbContext.Posts.ToListAsync();
            return Ok(posts);      
        }
        [HttpGet]
        [Route("{id:int}")]
        [ActionName("GetPostById")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post=await dbContext.Posts.FirstOrDefaultAsync(x=>x.Id==id);
            if (post != null)
            {
                return Ok(post);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(AddPostRequest addPostRequest)
        {
            //Convert DTO to Entity
            //await dbContext.Posts.AddAsync(post);
            //await dbContext.SaveChangesAsync();
            //return Ok(post);
            var post = new Post()
            {
                Title = addPostRequest.Title,
                Content = addPostRequest.Content,
                Author = addPostRequest.Author,
                FeaturedImagesUrl = addPostRequest.FeaturedImagesUrl,
                PublishedDate = addPostRequest.PublishedDate,
                Summary = addPostRequest.Summary,
                UrlHandle = addPostRequest.UrlHandle,
                Visible = addPostRequest.Visible,
            };
            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdatePostRequest(int id,UpdatePostRequests updatePostRequests)
        {
            //Convert DTO to Entity
            //Check if Exists
            var existingPost=await dbContext.Posts.FindAsync(id);
            if(existingPost != null)
            {
                existingPost.Title = updatePostRequests.Title;
                existingPost.Content = updatePostRequests.Content;
                existingPost.Author=updatePostRequests.Author;
                existingPost.FeaturedImagesUrl = updatePostRequests.FeaturedImagesUrl;
                existingPost.PublishedDate = updatePostRequests.PublishedDate;
                existingPost.Summary = updatePostRequests.Summary;
                existingPost.UrlHandle = updatePostRequests.UrlHandle;
                existingPost.Visible = updatePostRequests.Visible;
                existingPost.UpdatedDate = updatePostRequests.UpdatedDate;
                await dbContext.SaveChangesAsync();
                return Ok(existingPost);
            }
            return NotFound();

        }
        [HttpDelete]
        [Route("{id}")]
        public async Task <IActionResult> DeletePost(int id)
        {
            var existingpost=await dbContext.Posts.FindAsync(id);
            if(existingpost != null)
            {
                dbContext.Remove(existingpost);
                await dbContext.SaveChangesAsync();
                return Ok(existingpost);
            }
            return NotFound();

        }

    }
}
