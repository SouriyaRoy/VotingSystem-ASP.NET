using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using votingSystem.Controllers;
using votingSystem.Models;
using votingSystem.ExceptionFilter;
using System.Data.Entity;
using NUnit.Framework;

namespace VotingTest
{
    class ModifyExistingCandidateTests: DbContext
    {
        [Test]
        public void ModifyExistingCandidate_Successfull()
        {
            CandidateController candidateController = new CandidateController();
            Candidate candidate = new Candidate() { Name = "Omkar", DOB = System.DateTime.Today, PollId = 17, CandidateId = 35 };

            IHttpActionResult ii = candidateController.ModifyExistingCandidate(candidate);

            var content = ii as OkNegotiatedContentResult<string>;
            Assert.AreEqual("data entered successfully", content.Content);
        }

       
    }
}
