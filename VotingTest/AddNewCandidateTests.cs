using NUnit.Framework;
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

namespace VotingTest
{
    [TestFixture]
    public class AddNewCandidateTests: DbContext
    {
        [Test]
        public void AddNewCandidate_Successfull()
        {
            CandidateController candidateController = new CandidateController();
            Candidate candidate=new Candidate() {Name="abc", DOB = System.DateTime.Today, PollId = 17 };

            IHttpActionResult ii = candidateController.AddNewCandidate(candidate);

            var content = ii as OkNegotiatedContentResult<string>;
            Assert.AreEqual("Data Entered Successfully", content.Content);
        }
    }
}
