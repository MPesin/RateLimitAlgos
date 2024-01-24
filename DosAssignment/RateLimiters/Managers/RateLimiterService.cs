namespace DosAssignment.RateLimiters.Managers;

public static class RateLimiterService
{
    private static readonly ClientLimitManager StaticClientLimitManager = new();
    private static readonly ClientLimitManager DynamicClientLimitManager = new();
    private static RateLimitConfig _configuration = new(0, TimeSpan.Zero);

    public static void Configure(RateLimitConfig configuration)
    {
        _configuration = configuration;
    }

    public static Task SetDynamicRequestLimitAsync(int clientId)
    {
        return DynamicClientLimitManager.SetRequestAsync<SlidingRateLimiter>(clientId, _configuration);
    }

    public static Task SetStaticRequestLimitAsync(int clientId)
    {
        return StaticClientLimitManager.SetRequestAsync<StaticRateLimiter>(clientId, _configuration);
    }
}