namespace DosAssignment.RateLimiters.Managers;

public record RateLimitConfig(int MaxRequests, TimeSpan WindowTime);