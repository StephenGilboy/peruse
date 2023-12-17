using System.Diagnostics.Metrics;

namespace Peruse.Api.Wttw;

public class WttwMetrics
{
    private readonly Counter<int> _sessionCount;
    private readonly Counter<int> _pageNavigationCount;

    public WttwMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create("Peruse.Wttw");
        _sessionCount = meter.CreateCounter<int>("peruse.wttw.sessions.count");
        _pageNavigationCount = meter.CreateCounter<int>("peruse.wttw.page.navigation.count");
    }
    
    public void AddSession(Guid sessionId)
    {
        var tags = new KeyValuePair<string, object?>[]
        {
            new("session_id", sessionId),
            new("session_status", "started")
        };
        _sessionCount.Add(1, tags);
    }
    
    public void RemoveSession(Guid sessionId)
    {
        var tags = new KeyValuePair<string, object?>[]
        {
            new("session_id", sessionId),
            new("session_status", "ended")
        };
        _sessionCount.Add(1, tags);
    }
    
    public void NavigateToPage(Guid sessionId, Uri uri)
    {
        var tags = new KeyValuePair<string, object?>[]
        {
            new("session_id", sessionId),
            new("uri", uri)
        };
        _pageNavigationCount.Add(1, tags);
    }
}