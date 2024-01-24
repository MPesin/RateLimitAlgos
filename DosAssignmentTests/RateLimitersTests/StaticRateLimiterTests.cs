using DosAssignment.RateLimiters;
using DosAssignment.RateLimiters.Exceptions;

namespace DosAssignmentTests.RateLimitersTests;

public class StaticRateLimiterTests
{
    [Fact]
    public async void SetRequest_AllowsFiveRequestsInFiveSeconds_True()
    {
        const int totalRequests = 5;
        var timeWindow = TimeSpan.FromMilliseconds(5000);
        IRateLimiter limiter = new StaticRateLimiter(totalRequests, timeWindow);
        var success = true;
        for (var i = 0; i < totalRequests; i++)
        {
            try
            {
                await limiter.SetRequestAsync();
            }
            catch (RequestLimitReachedException)
            {
                success = false;
            }
            finally
            {
                await Task.Delay(500);
            }
        }
        
        Assert.True(success);
    }
    
    [Fact]
    public async void SetRequest_IncrementTotalSetRequestsCount_CounterIncrements()
    {
        const int totalRequests = 5;
        var timeWindow = TimeSpan.FromMilliseconds(5000);
        IRateLimiter limiter = new StaticRateLimiter(totalRequests, timeWindow);
        var totalRequestsSet = limiter.TotalRequestsSet;
        await limiter.SetRequestAsync();
        Assert.True(totalRequestsSet - limiter.TotalRequestsSet == 1);
    }
}