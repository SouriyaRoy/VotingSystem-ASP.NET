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
    public class PollController : ApiController
    {
        PollRepo rep = new PollRepo();

        [HttpGet]
        [Authorize(Roles = "1")]
        [Route("Poll/GetAllPolls")]
        public IHttpActionResult ReturnAll()
        {
            try
            {
                return Ok(rep.GetAll());
            }
            catch (Exception)
            {
                throw new CandidateNotFoundException("CanNot find any candidate");
            }
        }

        [NotFound]
        [HttpGet]
        [Authorize(Roles = "1")]
        [Route("Poll/GetPollDetails/{id}")]
        public IHttpActionResult DetailOfParticularPoll(int id)
        {
            Poll poll = rep.GetDetails(id);
            if (poll == null)
            {
                throw new PollNotFoundException("Invalid ID!!!!");
            }
            else
            {
                return Ok(poll);
            }
        }

        [NotAdded]
        [HttpPost]
        [Route("Poll/AddNewPoll")]
        [Authorize(Roles = "1")]
        public IHttpActionResult AddNewPoll(Poll poll)
        {
            if (ModelState.IsValid)
            {
                rep.CreatePoll(poll);
                return Ok("data entered successfully");
            }
            else
            {
                throw new WrongDataException("Data not entered correctly");
            }
        }

        [NotUpdate]
        [NotAdded]
        [HttpPut]
        [Route("Poll/EditPolldetails")]
        [Authorize(Roles = "1")]
        public IHttpActionResult ModifyExistingPoll(Poll poll)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    rep.updatePoll(poll);
                    return Ok("data entered successfully");
                }
                catch (Exception)
                {
                    throw new CanNotUpdateException("Update Failed!! Try Again");
                }
            }
            else
            {
                throw new WrongDataException("Data not entered correctly");
            }
        }

        [NotDeleted]
        [HttpDelete]
        [Route("Poll/DeletePoll/{id}")]
        [Authorize(Roles = "1")]
        public IHttpActionResult RemovePoll(int id)
        {
            try
            {
                rep.DeletePoll(id);
                return Ok("Poll Deleted Successfully");
            }
            catch (Exception)
            {
                throw new CanNotDeleteException("Error!! Try Again");
            }
        }
        
        [NotFound]
        [HttpGet]
        [Route("Poll/GetOptionsForPoll/{id}")]
        [Authorize(Roles = "1")]
        public IHttpActionResult GetOptionsForParticularPoll(int id)
        {
            try
            {
                return Ok(rep.GetResultModel(id));
            }
            catch (Exception)
            {
                throw new OptionNotFoundException("No Options for this poll !!!!");
            }
        }

        [NotFound]
        [HttpGet]
        [Route("Poll/GetCount")]
        public IHttpActionResult GetPollCount()
        {
            try
            {
                int pollCount = rep.GetCount();
                return Ok(pollCount);
            }
            catch (Exception)
            {
                throw new PollNotFoundException("Cannot found the counts for this poll !!!!");
            }
        }

        [NotFound]
        [HttpGet]
        [Authorize(Roles = "0,1")]
        [Route("Poll/GetAllPollOptions")]
        public IHttpActionResult GetAllPollsWithOptions()
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity as ClaimsIdentity;
                string s = identity.FindFirst("ID").Value;
                int userId = Convert.ToInt32(s);

                List<Poll> polls = ShowActivePollsForUser(userId);
                return Ok(rep.GetAllPollsWithOptions(polls));
            }
            catch (Exception)
            {
                throw new PollNotFoundException("No Polls to show !!!!");
            }
        }

        [HttpGet]
        [Route("Poll/ClosePoll/{Id}")]
        [Authorize(Roles = "1")]
        public IHttpActionResult ClosePoll(int Id)
        {
            List<Option> _options = rep.ClosePoll(Id);
            return Ok(_options);
        }

        // To get all closed polls with options (For admin)
        [NotFound]
        [HttpGet]
        [Route("Poll/GetAllClosedPolls/{id}")]
        [Authorize(Roles = "1")]
        public IHttpActionResult GetAllClosedPolls(int id)
        {
            try
            {
                return Ok(rep.GetResultModel(id));
                return Ok();
            }
            catch (Exception)
            {
                throw new PollNotFoundException("NO closed Polls");
            }
        }

        [NotFound]
        [HttpGet]
        [Route("Poll/GetActivePolls")]
        public IHttpActionResult ActivePolls()
        {
            try 
            {
                List<Poll> polls = rep.GetActivePolls();
                return Ok(polls);
            }
            catch (Exception)
            {
                throw new PollNotFoundException("No Active Polls");
            }
        }

        [HttpGet]
        [NonAction]
        public List<Poll> ShowActivePollsForUser(int userId)
        {
            try
            {
                List<Poll> polls = rep.ShowActivePollsForUser(userId);
                return polls;
            }
            catch (Exception)
            {
                return new List<Poll>();
            }
        }
    }
}