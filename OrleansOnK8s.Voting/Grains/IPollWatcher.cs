namespace OrleansOnK8s.Voting.Grains;

public interface IPollWatcher : IGrainObserver
{
    void OnPollUpdated(PollState state);
}
