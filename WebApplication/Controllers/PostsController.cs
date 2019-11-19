using System.Linq;
using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverFlow;
using WebServiceToken.Models;

namespace WebServiceToken.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController : Controller
    {
        /*private readonly IDataService _dataService;

        public PostsController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("{postId}")]
        public ActionResult<Post> GetCategory(int postId)
        {
            var post = _dataService.GetPost(postId);

            if (post == null) return NotFound();

            return Ok(post);
        }

        /*[Authorize]
        [HttpGet]
        public IActionResult GetPosts()
        {
            int.TryParse(HttpContext.User.Identity.Name, out var id);
            var posts = _dataService.GetPosts(id);

            var result = posts.Select(x => new PostDto { Title = x.Title });
            return Ok(result);
        }*/
    }
}
