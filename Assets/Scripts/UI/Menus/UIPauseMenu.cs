using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenu : Singleton<UIPauseMenu>
{
    public GameObject PauseMenu;
    private void Awake()
    {
        PauseMenu.SetActive(false);
    }

    public void TogglePause()
    {
        GameManager.Instance.TogglePauseState();
        PauseMenu.SetActive(GameManager.Instance.isPaused);
    }
}
