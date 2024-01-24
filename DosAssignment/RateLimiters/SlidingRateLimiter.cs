using DosAssignment.RateLimiters.Exceptions;

namespace DosAssignment.RateLimiters;

public class SlidingRateLimiter : IRateLimiter
{
    private readonly int _maxRequests;
    private readonly TimeSpan _timeWindow;
    private readonly Queue<DateTime> _requestsTimestampQueue;

    public SlidingRateLimiter(int maxRequests, TimeSpan timeWindow)
    {
        _requestsTimestampQueue = new Queue<DateTime>();
        _maxRequests = maxRequests;
        _timeWindow = timeWindow;
    }

    public Task SetRequestAsync()
    {
        var now = DateTime.Now;
        ClearQueueFromOldTimestamps(now);
        _requestsTimestampQueue.Enqueue(now);
        if (_requestsTimestampQueue.Count >= _maxRequests)
        {
            throw new RequestLimitReachedException();
        }

        return Task.CompletedTask;
    }

    private void ClearQueueFromOldTimestamps(DateTime now)
    {
        while (_requestsTimestampQueue.Any()
               && IsNextTimestampNotInWindow(now))
        {
            _requestsTimestampQueue.Dequeue();
        }
    }

    private bool IsNextTimestampNotInWindow(DateTime now)
    {
        var nextTimestamp = _requestsTimestampQueue.Peek();
        var timePassed = now.Subtract(nextTimestamp);
        return timePassed.CompareTo(_timeWindow) > 0;
    }

    public int TotalRequestsSet => _requestsTimestampQueue.Count;
}