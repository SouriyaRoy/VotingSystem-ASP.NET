using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using votingSystem.Models;

namespace votingSystem.Repositories
{
    public class OptionRepo
    {
        private PollingSystemEntities entity = new PollingSystemEntities();

        public void CreateOption(Option _option)
        {
            entity.Options.Add(_option);
            entity.SaveChanges();
        }

            public void IncreaseCount(int userId, int candidateId, int pollId)
        {
            Option _option = (from option in entity.Options.ToList()
                                where option.CandidateId == candidateId && option.PollId == pollId
                                select option).SingleOrDefault();
            _option.Count++;
            UserHistory userHistory = new UserHistory() { UserId = userId, PollId = pollId};
            entity.UserHistories.Add(userHistory);
            entity.SaveChanges();
        }
    }
}