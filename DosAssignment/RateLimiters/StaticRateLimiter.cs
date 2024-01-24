using DosAssignment.RateLimiters.Exceptions;

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

    public int TotalRequestsSet { get; private set; }

    public Task SetRequestAsync()
    {
        var nextValue = TotalRequestsSet + 1;
        TotalRequestsSet = nextValue <= _totalRequests
            ? nextValue
            : throw new RequestLimitReachedException();
        return Task.CompletedTask;
    }
}