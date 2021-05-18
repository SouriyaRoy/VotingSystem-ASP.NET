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
    class ModifyExistingOptionTests
    {
        [Test]
        public void ModifyExistingOptionTests_Successfull()
        {
            OptionController optionController = new OptionController();
            Option option = new Option() { OptionId = 17, PollId = 20, CandidateId =35 };

            IHttpActionResult ii = optionController.ModifyExistingOption(option);
            var content = ii as OkNegotiatedContentResult<string>;

            Assert.AreEqual("Data Updated Successfully", content.Content);
        }
    }
}
