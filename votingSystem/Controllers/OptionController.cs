using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using votingSystem.ExceptionFilter;
using votingSystem.Models;
using votingSystem.Repositories;

namespace votingSystem.Controllers.API
{
    public class OptionController : ApiController
    {
        private OptionRepo rep = new OptionRepo();

        [HttpGet]
        [Route("Option/IncreaseCount/{candidateId}/{pollId}")]
        [Authorize(Roles = "0,1")]
        public IHttpActionResult IncreaseCountOfOption(int candidateId, int pollId)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity as ClaimsIdentity;
                string s = identity.FindFirst("ID").Value;
                int userId = Convert.ToInt32(s);

                rep.IncreaseCount(userId, candidateId, pollId);
                return Ok("Vote Incremented Succussfully !");
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
