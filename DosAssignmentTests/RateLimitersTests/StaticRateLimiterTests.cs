using DosAssignment.RateLimiters;
using DosAssignment.RateLimiters.Exceptions;

namespace DosAssignmentTests.RateLimitersTests;

public class StaticRateLimiterTests
{
    [Fact]
    public async void SetRequestAsync_AllowsFiveRequestsInFiveSeconds_True()
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
    public async void SetRequestAsync_IncrementTotalSetRequestsCount_CounterIncrements()
    {
        const int totalRequests = 1;
        var timeWindow = TimeSpan.FromMilliseconds(5000);
        IRateLimiter limiter = new StaticRateLimiter(totalRequests, timeWindow);
        var totalRequestsSetPreCall = limiter.TotalRequestsSet;
        await limiter.SetRequestAsync();
        Assert.True(limiter.TotalRequestsSet - totalRequestsSetPreCall == 1);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(10)]
    public async void SetRequestAsync_AddingRequestOverLimitFails_ThrowsRequestLimitReachedException(int totalRequests)
    {
        var timeWindow = TimeSpan.FromMilliseconds(5000);
        IRateLimiter limiter = new StaticRateLimiter(totalRequests, timeWindow);
        for (var i = 0; i < totalRequests; i++)
        {
            await limiter.SetRequestAsync();
        }

        await Assert.ThrowsAsync<RequestLimitReachedException>(() => limiter.SetRequestAsync());
    }

    [Theory]
    [InlineData(500)]
    [InlineData(1000)]
    public async void SetRequestAsync_AddingOneRequestOverLimitAfterWindow_TotalRequestsEqualsOne(int windowMilliseconds)
    {
        const int totalRequests = 1;
        var timeWindow = TimeSpan.FromMilliseconds(windowMilliseconds);
        IRateLimiter limiter = new StaticRateLimiter(totalRequests, timeWindow);
        await limiter.SetRequestAsync();
        await Task.Delay(timeWindow);
        await limiter.SetRequestAsync();
        Assert.True(limiter.TotalRequestsSet == 1);
    }
}