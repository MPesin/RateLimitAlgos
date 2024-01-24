namespace DosAssignment.RateLimiters;

public class StaticRateLimiter : IRateLimiter
{
    private readonly int _totalRequests;
    private readonly TimeSpan _timeWindow;

    public StaticRateLimiter(int totalRequests, TimeSpan timeWindow)
    {
        _totalRequests = totalRequests;
        _timeWindow = timeWindow;
    }

    public Task SetRequestAsync()
    {
        throw new NotImplementedException();
    }

    public bool SetRequest()
    {
        throw new NotImplementedException();
    }
}