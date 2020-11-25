using System;
using UnityEngine;

public class Timer
{
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
    }
    public void FinishTimer()
    {
        timer=threshold+1;
    }
}