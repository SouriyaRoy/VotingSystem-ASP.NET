using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using votingSystem.Models;

namespace votingSystem.ViewModels
{
    public class PollResultViewModel
    {
        public Poll poll { get; set; }
        public IEnumerable<Option> Option { get; set; }

        public IEnumerable<Candidate> Candidates { get; set; }
    }
}