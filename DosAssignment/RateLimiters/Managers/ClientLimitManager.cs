namespace DosAssignment.RateLimiters.Managers;

internal class ClientLimitManager
{
    private readonly IDictionary<int, IRateLimiter> _clientRateLimiters = new Dictionary<int, IRateLimiter>();
    private readonly AutoResetEvent _resetEvent = new(true);
    public bool ClientExists(int clientId) => _clientRateLimiters.ContainsKey(clientId);

    public void AddClient(int clientId, IRateLimiter rateLimiter) => _clientRateLimiters.Add(clientId, rateLimiter);

    public Task SetRequestAsync<T>(int clientId, RateLimitConfig config) where T : IRateLimiter
    {
        try
        {
            _resetEvent.WaitOne();
            if (!ClientExists(clientId))
            {
                var limiter = CreateRateLimiter<T>(config);
                AddClient(clientId, limiter!);
            }

            return _clientRateLimiters[clientId].SetRequestAsync();
        }
        finally
        {
            _resetEvent.Set();
        }
    }

    private static IRateLimiter? CreateRateLimiter<T>(RateLimitConfig config) where T : IRateLimiter
    {
        IRateLimiter? limiter = null;
        if (typeof(T) == typeof(StaticRateLimiter))
        {
            limiter = new StaticRateLimiter(config.MaxRequests, config.WindowTime);
        }
        else if (typeof(T) == typeof(SlidingRateLimiter))
        {
            limiter = new SlidingRateLimiter(config.MaxRequests, config.WindowTime);
        }

        return limiter;
    }
}