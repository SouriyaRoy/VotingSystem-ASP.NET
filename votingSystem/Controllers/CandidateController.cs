using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using votingSystem.ExceptionFilter;
using votingSystem.Models;
using votingSystem.Repositories;

namespace votingSystem.Controllers
{
    public class CandidateController : ApiController
    {
        private CandidateRepo repo = new CandidateRepo();

        [NotFound]
        [HttpGet]
        [Route("Candidate/GetAllCandidates")]
        [Authorize(Roles ="1")]
        public IHttpActionResult GetReturnAll()
        {
            try
            {
                return Ok(repo.GetAllCandidates());
            }
            catch(Exception)
            {
                throw new CandidateNotFoundException("Can not find any candidate!!");
            }
        }

        [NotFound]
        [HttpGet]
        [Route("Candidate/CandidateDetails/{id}")]
        [Authorize(Roles = "1")]
        public IHttpActionResult DetailOfParticularCandidate(int id)
        {
            Candidate c = repo.GetDetails(id);
            if (c == null)
            {
                throw new CandidateNotFoundException("Invalid ID!!!!");
            }
            else
            {
                return Ok(c);
            }
        }

        [NotAdded]
        [HttpPost]
        [Route("Candidate/AddNewCandidate")]
        [Authorize(Roles = "1")]
        public IHttpActionResult AddNewCandidate(Candidate value)
        {
            if (ModelState.IsValid)
            {
                repo.CreateCandidate(value);
                return Ok("Data Entered Successfully");
            }
            else
            {
                throw new WrongDataException("Data not entered correctly");
            }
        }

        [NotUpdate]
        [NotAdded]
        [HttpPut]
        [Route("Candidate/Editdetails")]
        [Authorize(Roles = "1")]
        public IHttpActionResult ModifyExistingCandidate(Candidate c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    repo.UpdateCandidate(c);
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
        [Route("Candidate/DeleteCandidate/{id}")]
        [Authorize(Roles = "1")]
        public IHttpActionResult RemoveCandidate(int id)
        {
            try
            {
                repo.DeleteCandidate(id);
                return Ok("Candidate Deleted Successfully");
            }
            catch (Exception)
            {
                throw new CanNotDeleteException("Error!! Try Again");
            }
        }
    }
}
