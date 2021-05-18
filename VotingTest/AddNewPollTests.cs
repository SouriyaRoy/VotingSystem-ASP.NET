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
using votingSystem.Controllers.API;

namespace VotingTest
{
    [TestFixture]
    class AddNewPollTests
    {
        [Test]
        public void AddNewPoll_Successfull()
        {
            PollController pollController = new PollController();
            Poll poll = new Poll() { PollId = 2, PollName = "leader", Result = "declared" };

            IHttpActionResult ii = pollController.AddNewPoll(poll);
            var content = ii as OkNegotiatedContentResult<string>;

            Assert.AreEqual("data entered successfully", content.Content);
        }
    }
}
