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
        public ActionResult<Question> GetDetailQuestion([FromBody]int postId)
        {
            var answer = _dataService.getAnswer(postId);

            if (answer != null)
            {
                postId = answer.QuestionId;
            }
            
            var question = _dataService.GetDetailQuestion(postId);

            if (question == null) return NotFound();

            return Ok(question);
        }
    }
}
