using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenu : Singleton<UIPauseMenu>
{
    public GameObject PauseMenu;
    public bool isPaused;
    private void Awake()
    {
        PauseMenu.SetActive(false);
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
    }

    public void UnPause()
    {
        
        PauseMenu.SetActive(false);
    }
    
}
