namespace DosAssignment.RateLimiters;

public interface IRateLimiter
{
    Task SetRequestAsync();
    int TotalRequestsSet { get; }
}