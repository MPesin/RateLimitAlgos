namespace DosAssignment.RateLimiters;

public interface IRateLimiter
{
    Task SetRequestAsync();
}