using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour, Service.IGameService
{
    public Settings Settings;

    public InputSettings GetInputSettings()
    {
        return Settings.InputSettings;
    }
}
