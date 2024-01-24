namespace DosAssignment.RateLimiters;

public class SlidingRateLimiter : IRateLimiter
{
    public Task SetRequestAsync()
    {
        throw new NotImplementedException();
    }

    public int TotalRequestsSet { get; }
}