using PollingSystemMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using votingSystem.Models;
using votingSystem.ViewModels;

namespace votingSystem.Repositories
{
    public class PollRepo
    {
        private PollingSystemEntities entity = new PollingSystemEntities();

        public List<Poll> GetAll()
        {
            return entity.Polls.ToList();
        }

        public Poll GetDetails(int poll_Id)
        {
            Poll _poll = entity.Polls.Find(poll_Id);
            return _poll;
        }

        public void CreatePoll(Poll _poll)
        {
            entity.Polls.Add(_poll);
            entity.SaveChanges();
        }

        public void DeletePoll(int poll_Id)
        {
            IEnumerable<Option> _options = entity.Options.Where(o => o.PollId == poll_Id);
            entity.Options.RemoveRange(_options);

            IEnumerable<Candidate> _candidates = entity.Candidates.Where(o => o.PollId == poll_Id);
            entity.Candidates.RemoveRange(_candidates);

            Poll _poll = entity.Polls.Find(poll_Id);

            IEnumerable<UserHistory> histories = entity.UserHistories.Where(o => o.PollId == poll_Id);
            entity.UserHistories.RemoveRange(histories);

            entity.Polls.Remove(_poll);
            entity.SaveChanges();
        }


        public void updatePoll(Poll _poll)
        {
            entity.Entry(_poll).State = EntityState.Modified;
            entity.SaveChanges();
        }

        public IEnumerable<string> GetOptionsForParticularPoll(int poll_Id)
        {
            IEnumerable<string> candidateNames = from candidate in entity.Candidates.ToList()
                                                 join option in entity.Options.ToList()
                                                 on candidate.CandidateId equals option.CandidateId
                                                 where option.PollId == poll_Id
                                                 select candidate.Name;
            return candidateNames;
        }

        public List<Candidate> GetOptionsForParticularPollAdmin(int poll_Id)
        {
            IEnumerable<Candidate> candidateNames = from candidate in entity.Candidates.ToList()
                                                 where candidate.PollId == poll_Id
                                                    select candidate;
            List<Candidate> toBeSent = new List<Candidate>();
            Candidate candidate1;
            foreach (var item in candidateNames.ToList())
            {
                candidate1 = new Candidate();
                candidate1.CandidateId = item.CandidateId;
                candidate1.Name = item.Name;
                toBeSent.Add(candidate1);
            }
            return toBeSent;
        }

        public int GetCount()
        {
            return entity.Polls.Count();
        }

        public IEnumerable<PollDetailsViewModel> GetAllPollsWithOptions(List<Poll> activePolls)
        {
            List<PollDetailsViewModel> allPollsWithOption = new List<PollDetailsViewModel>();
            List<Poll> allPolls = GetAll();
            int numOfPolls = allPolls.Count;
            PollDetailsViewModel pollDetailsViewModel;
            for (int i = 0; i < numOfPolls; ++i)
            {
                pollDetailsViewModel = new PollDetailsViewModel();

                Poll _poll = new Poll();
                _poll.PollId = allPolls[i].PollId;

                List<int> pollsId = activePolls.Select(poll => poll.PollId).ToList();
                if (!pollsId.Contains(_poll.PollId))
                    continue;

                _poll.PollStatus = allPolls[i].PollStatus;

                if (_poll.PollStatus == true)
                    continue;

                _poll.PollName = allPolls[i].PollName;
                _poll.Result = allPolls[i].Result;
                
                int pollid = allPolls[i].PollId;

                List<Option> options = entity.Options.Where(o => o.PollId == pollid).ToList();
                List<Option> optionsToSend = new List<Option>();

                int count = options.Count();
                for (int j = 0; j < count; ++j)
                {
                    Option oldObj = options[j];
                    Option newObj = new Option();
                    newObj.OptionId = oldObj.OptionId;
                    newObj.PollId = oldObj.PollId;
                    newObj.CandidateId = oldObj.CandidateId;
                    newObj.Count = oldObj.Count;
                    optionsToSend.Add(newObj);
                }

                pollDetailsViewModel.OptionsWithString = GetOptionsForParticularPoll(_poll.PollId).ToList();

                pollDetailsViewModel.Polls = _poll;
                pollDetailsViewModel.Option = optionsToSend;
                allPollsWithOption.Add(pollDetailsViewModel);
            }
            return allPollsWithOption;
        }

        public IEnumerable<PollResultViewModel> GetAllClosedPollsWithOptions()
        {
            List<PollResultViewModel> allPollsWithOption = new List<PollResultViewModel>();
            List<Poll> allPolls = GetAll();
            int numOfPolls = allPolls.Count;
            PollResultViewModel pollDetailsViewModel;
            for (int i = 0; i < numOfPolls; ++i)
            {
                pollDetailsViewModel = new PollResultViewModel();

                Poll _poll = new Poll();
                _poll.PollId = allPolls[i].PollId;
                _poll.PollStatus = allPolls[i].PollStatus;


                if (_poll.PollStatus == false)
                    continue;

                _poll.PollName = allPolls[i].PollName;
                _poll.Result = allPolls[i].Result;

                int pollid = allPolls[i].PollId;

                List<Option> options = entity.Options.Where(o => o.PollId == pollid).ToList();
                List<Option> optionsToSend = new List<Option>();

                int count = options.Count();
                for (int j = 0; j < count; ++j)
                {
                    Option oldObj = options[j];
                    Option newObj = new Option();
                    newObj.OptionId = oldObj.OptionId;
                    newObj.PollId = oldObj.PollId;
                    newObj.CandidateId = oldObj.CandidateId;
                    newObj.Count = oldObj.Count;

                    optionsToSend.Add(newObj);
                }

                pollDetailsViewModel.Candidates = GetOptionsForParticularPollAdmin(_poll.PollId).ToList();

                pollDetailsViewModel.poll = _poll;
                pollDetailsViewModel.Option = optionsToSend;
                allPollsWithOption.Add(pollDetailsViewModel);
            }
            return allPollsWithOption;
        }

        public PollResultViewModel GetResultModel(int id)
        {
            Poll p = entity.Polls.ToList().Find(i => i.PollId == id);
            IEnumerable<Option> op = entity.Options.ToList().FindAll(i => i.PollId == id);
            IEnumerable<Candidate> cand = entity.Candidates.ToList().FindAll(i => i.PollId == id);

            Poll pollToBeSent = new Poll() { PollId = p.PollId, PollStatus = p.PollStatus, PollName = p.PollName, Result = p.Result };

            List<Option> optionListToBeSent = new List<Option>() ;
            foreach (Option item in op)
            {
                Option option = new Option() { OptionId = item.OptionId, CandidateId = item.CandidateId, PollId = item.PollId, Count = item.Count};
                optionListToBeSent.Add(option);
            }

            List<Candidate> candidateListToBeSent = new List<Candidate>();
            foreach (Candidate item in cand)
            {
                Candidate candidate = new Candidate() { CandidateId = item.CandidateId, Name = item.Name, DOB = item.DOB, PollId = item.PollId };
                candidateListToBeSent.Add(candidate);
            }
            PollResultViewModel pollView = new PollResultViewModel() { poll = pollToBeSent, Option = optionListToBeSent, Candidates = candidateListToBeSent };

            return pollView;
        }


        public List<Option> ClosePoll(int poll_Id)
        {
            Poll _poll = entity.Polls.Find(poll_Id);
            _poll.PollStatus = !_poll.PollStatus;
            _poll.Result = "Declared";
            entity.SaveChanges();

            List<Option> _options = entity.Options.Where(o => o.PollId == poll_Id).ToList();

            List<Option> _optionToBeReturnd = new List<Option>();
            int count = _options.Count();
            for (int i = 0; i < count; ++i)
            {
                Option newObj = new Option();
                Option oldObj = _options[i];
                newObj.OptionId = oldObj.OptionId;
                newObj.PollId = oldObj.PollId;
                newObj.CandidateId = oldObj.CandidateId;
                newObj.Count = oldObj.Count;

                _optionToBeReturnd.Add(newObj);
            }
            return _optionToBeReturnd;
        }

        public List<Poll> GetActivePolls()
        {
            List<Poll> activePolls = entity.Polls.ToList().FindAll(i => i.PollStatus == true);
            return activePolls;
        }

        public List<Poll> ShowActivePollsForUser(int id)
        {
            List<Poll> list = entity.Polls.ToList();
            List<UserHistory> l = entity.UserHistories.ToList().FindAll(i => i.UserId == id);
            foreach (var i in l)
            {
                list.RemoveAll(j => j.PollId == i.PollId);
            }
            return list;
        }
    }
}