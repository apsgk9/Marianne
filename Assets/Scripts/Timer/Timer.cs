using System;
using UnityEngine;

public class Timer
{
    public event Action TimerReset;
    public float timer;
    public float threshold;
    public bool Activated=>timer>=threshold;
    public Timer(float thresholdInput)
    {
        threshold=thresholdInput;
    }
    public void Tick()
    {
        timer+=Time.deltaTime;
    }
    public void ResetTimer()
    {
        timer=0;
        TimerReset?.Invoke();
    }
}