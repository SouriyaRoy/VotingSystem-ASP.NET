using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using voting.Models;

namespace voting.ViewModels
{
    public class PollDetailsViewModel
    {
        public Poll Polls { get; set; }
        public IEnumerable<Option> Option { get; set; }

        public IEnumerable<string> OptionsWithString { get; set; }
    }
}