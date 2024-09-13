using System.Diagnostics;
using Orleans.Providers;

namespace OrleansOnK8s.Voting.Grains;

[StorageProvider(ProviderName = "votes")]
public class VoteGrain(
    [PersistentState("votes", storageName: "votes")] IPersistentState<Dictionary<string, int>> state,
    ILogger<VoteGrain> logger)
    : Grain, IVoteGrain
{
    private readonly ILogger _logger = logger;

    public Task<Dictionary<string, int>> Get() => Task.FromResult(state.State);

    public async Task AddVote(string option)
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("Saving vote");

        var key = option.ToLower();
        if (!state.State.ContainsKey(key))
        {
            _logger.LogInformation("Created vote option {Option} and voted...", option);
            state.State.Add(key, 1);
        }
        else
        {
            _logger.LogInformation("Voting for {Option}...", option);
            state.State[key] += 1;
        }

        await state.WriteStateAsync();
        _logger.LogInformation("Saved vote in {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
    }

    public async Task RemoveVote(string option)
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("Deleting vote option");

        var key = option.ToLower();
        if (!state.State.ContainsKey(key))
        {
            _logger.LogWarning("Didn't find vote option {Option}", key);
            throw new KeyNotFoundException($"Didn't find vote option {key}");
        }
        else
        {
            _logger.LogInformation("Removed vote option {Option}...", key);
            state.State.Remove(key.ToLower());
        }

        await state.WriteStateAsync();

        _logger.LogInformation("Deleted vote option {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
    }
}
