using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackOverFlow;
using WebServiceToken.Models;
using WebServiceToken.Services;

namespace WebServiceToken.Controllers
{
    [ApiController]
    [Route("api/mark")]
    public class MarkingController : Controller
    {
        private readonly IDataService _dataService;

        public MarkingController(IDataService dataService, IConfiguration configuration)
        {
            _dataService = dataService;
        }

        [HttpPost("markings")]
        public ActionResult GetMarkings([FromBody]User user)
        {
            var markings = _dataService.GetMarkings(user.UserName);
            
            if (markings == null) return NotFound();

            return Ok(markings);
        }
        
        [HttpPost]
        public ActionResult<Marking> CreateMarking([FromBody] Marking marking)
        {
            var categoryCreated = _dataService.CreateMarking(marking.Annotation, marking.Username,marking.PostId,marking.CommentId,DateTime.Now);;

            return Created("Created Marking",categoryCreated);
        }

        
        [HttpPut]
        public ActionResult<Marking> UpdateCategory([FromBody] Marking marking)
        {
            if (!_dataService.UpdateMarking(marking.Id, marking.Annotation))
                return NotFound();
            return Ok(new Marking(){Id = marking.Id, Annotation = marking.Annotation});
        }
        
        [HttpDelete("{markingid}")]
        public ActionResult DeleteMarking(int markingid)
        {
            if (!_dataService.DeleteMarking(markingid))
                return NotFound();
            return Ok();
        }
    }
}