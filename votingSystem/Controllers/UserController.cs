using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using votingSystem.Models;
using votingSystem.repositories;

namespace votingSystem.Controllers
{
    [RoutePrefix("User")]
    public class UserController : ApiController
    {
        private UserRepo repo = new UserRepo();

        [HttpPost]
        [Route("AddNewUser")]
        public IHttpActionResult Register(User user)
        {
            repo.Register(user);
            return Ok();
        }

        [HttpGet]
        [Route("GetUserType")]
        [Authorize(Roles ="0,1")]
        public IHttpActionResult GetUserType()
        {
            var identity = (ClaimsIdentity)User.Identity as ClaimsIdentity;
            IEnumerable<string> roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            IEnumerable<Claim> claims = identity.Claims;
            return Ok(roles);
        }
    }
}