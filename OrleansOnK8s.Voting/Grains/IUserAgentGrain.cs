namespace OrleansOnK8s.Voting.Grains;

public interface IUserAgentGrain : IGrainWithStringKey
{
    Task<string> CreatePoll(PollState initialState);
    Task<(PollState Results, bool Voted)> GetPollResults(string pollId);
    Task<PollState> AddVote(string pollId, int optionId);
}
