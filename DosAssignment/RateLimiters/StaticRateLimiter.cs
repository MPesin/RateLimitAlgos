using System.Timers;
using DosAssignment.RateLimiters.Exceptions;
using Timer = System.Timers.Timer;

namespace DosAssignment.RateLimiters;

public class StaticRateLimiter : IRateLimiter, IDisposable
{
    private readonly int _maxRequests;
    private readonly Timer _windowTimer;
    private readonly AutoResetEvent _resetEvent = new(true);
    private bool _disposed;

    public StaticRateLimiter(int maxRequests, TimeSpan timeWindow)
    {
        _maxRequests = maxRequests;
        _windowTimer = new Timer(timeWindow);
        _windowTimer.Elapsed += OnWindowTimerElapsed;
    }

    private void OnWindowTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _resetEvent.WaitOne();
        TotalRequestsSet = 0;
        _resetEvent.Set();
    }

    public int TotalRequestsSet { get; private set; }

    public Task SetRequestAsync()
    {
        try
        {
            _resetEvent.WaitOne();
            HandleFirstRequestInWindow();
            var nextValue = TotalRequestsSet + 1;
            TotalRequestsSet = nextValue <= _maxRequests
                ? nextValue
                : throw new RequestLimitReachedException();
        }
        finally
        {
            _resetEvent.Set();
        }

        return Task.CompletedTask;
    }

    private void HandleFirstRequestInWindow()
    {
        if (TotalRequestsSet == 0)
        {
            _windowTimer.Start();
        }
    }

    #region Dispose Pattern

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _resetEvent.Dispose();
            _windowTimer.Dispose();
        }

        _disposed = true;
    }

    #endregion
}