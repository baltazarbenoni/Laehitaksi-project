using UnityEngine;

public class Timer
{
    public Timer(float limit)
    {
        this.limit = limit;
    }
    public Timer()
    {
        this.limit = 0.5f;
    }
    float time;
    internal float Time { get; set; }
    protected float limit;
    internal float Limit
    {
        get
        {
            return limit;
        }
        set
        {
            limit = value;
        }
    }
}
public class WaitTimer : Timer
{
    public WaitTimer()
    {
        limit = 0;
    }
    public WaitTimer(int maxWaitTime)
    {
        this.limit = maxWaitTime;
    }
    bool waitHasEnded;
    internal bool WaitHasEnded
    {
        get
        {
            return waitHasEnded;
        }
        set
        {
            waitHasEnded = value;
        }
    }

}

