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
        private readonly IDataService _dataService;

        public PostsController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost]
        public ActionResult<Question> GetDetailQuestion([FromBody]Question post)
        {
            var answer = _dataService.getAnswer(post.Id);

            if (answer != null)
            {
                post.Id = answer.QuestionId;
            }
            
            var question = _dataService.GetDetailQuestion(post.Id);

            if (question == null) return NotFound();

            return Ok(question);
        }
        
        [HttpPost("search")]
        public ActionResult GetSearch([FromBody]TextForPost searchtext)
        {
            var search = _dataService.searchPosts(searchtext.SearchText);
            
            if (search == null) return NotFound();

            return Ok();
        }
    }
}
