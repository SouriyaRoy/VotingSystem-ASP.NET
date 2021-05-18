using votingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using votingSystem.ViewModels;

namespace PollingSystemMVC.ViewModels
{
    public class PollDetailsViewModel
    {
        public Poll Polls { get; set; }
        public IEnumerable<Option> Option { get; set; }

        public IEnumerable<string> OptionsWithString { get; set; }

        public static implicit operator PollDetailsViewModel(PollResultViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}