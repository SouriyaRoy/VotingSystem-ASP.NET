using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using votingSystem.Models;

namespace votingSystem.Repositories
{
    public class CandidateRepo
    {
        private PollingSystemEntities entity = new PollingSystemEntities();
        
        public List<Candidate> GetAllCandidates()
        {
            return entity.Candidates.ToList();
        }

        public Candidate GetDetails(int candidateId)
        {
            Candidate _candidate = entity.Candidates.Find(candidateId);
            return _candidate;
        }


        public void CreateCandidate(Candidate _candidate)
        {
            entity.Candidates.Add(_candidate);
            entity.SaveChanges();

            int _pollId = _candidate.PollId;
            int _candidateId = _candidate.CandidateId;

            OptionRepo _repo = new OptionRepo();
            Option _option = new Option() { PollId = _pollId, CandidateId = _candidateId, Count = 0 };
            _repo.CreateOption(_option);
        }

        public void DeleteCandidate(int candidateId)
        {
            IEnumerable<Option> _options = entity.Options.Where(o => o.CandidateId == candidateId);
            entity.Options.RemoveRange(_options);

            Candidate _candidate = entity.Candidates.Find(candidateId);
            entity.Candidates.Remove(_candidate);
            entity.SaveChanges();
        }

        public void UpdateCandidate(Candidate _candidate)
        {
            entity.Entry(_candidate).State = System.Data.Entity.EntityState.Modified;
            entity.SaveChanges();
        }
    }
}