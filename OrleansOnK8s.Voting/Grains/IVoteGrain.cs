namespace OrleansOnK8s.Voting.Grains;

public interface IVoteGrain : IGrainWithIntegerKey
{
    Task<Dictionary<string, int>> Get();
    Task AddVote(string option);
    Task RemoveVote(string option);
}
