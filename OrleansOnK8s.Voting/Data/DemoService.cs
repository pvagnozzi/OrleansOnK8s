using OrleansOnK8s.Voting.Grains;

namespace OrleansOnK8s.Voting.Data;

public partial class DemoService(IGrainFactory grainFactory, ILogger<DemoService> logger)
{
    public async Task SimulateVoters(string pollId, int numVotes)
    {
        try
        {
            var pollGrain = grainFactory.GetGrain<IPollGrain>(pollId);
            var results = await pollGrain.GetCurrentResults();
            var random = Random.Shared;
            while (numVotes-- > 0)
            {
                var optionId = random.Next(0, results.Options.Count);
                await pollGrain.AddVote(optionId);

                // Wait some time.
                await Task.Delay(random.Next(100, 1000));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while simulating voters");
        }
    }
}