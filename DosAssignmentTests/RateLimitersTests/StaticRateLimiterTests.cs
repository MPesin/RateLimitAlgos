using DosAssignment.RateLimiters;

namespace DosAssignmentTests.RateLimitersTests;

public class StaticRateLimiterTests
{
    [Fact]
    public async void SetRequest_AllowsFiveRequestsInFiveSeconds_True()
    {
        const int totalRequests = 5;
        var timeWindow = TimeSpan.FromMilliseconds(5000);
        var limiter = new StaticRateLimiter(totalRequests, timeWindow);
        var success = false;
        for (var i = 0; i < totalRequests; i++)
        {
            success = limiter.SetRequest();
            await Task.Delay(500);
        }
        
        Assert.True(success);
    }
}