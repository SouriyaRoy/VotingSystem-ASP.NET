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
    class AddNewOptionTests
    {
        [Test]
        public void AddNewOption_Successfull()
        {
            OptionController optionController = new OptionController();
            Option option = new Option() { OptionId=17, PollId=17, CandidateId=35};

            IHttpActionResult ii = optionController.AddNewOption(option);

            var content = ii as OkNegotiatedContentResult<string>;
            Assert.AreEqual("data entered successfully", content.Content);
        }
    }
}
