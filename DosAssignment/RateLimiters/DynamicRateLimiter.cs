namespace DosAssignment.RateLimiters;

public class DynamicRateLimiter : IRateLimiter
{
    public Task SetRequestAsync()
    {
        throw new NotImplementedException();
    }

    public int TotalRequestsSet { get; }
}