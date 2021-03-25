using System;

public interface IRotateDesiredForwardEvent
{
    event Action OnCallDesiredForwardRotationChange;
}