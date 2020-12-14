using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCursorToScreen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private bool isCursorLocked=true;
    [SerializeField]
    private bool isConfined;

    [SerializeField]
    private KeyCode ConfineKey=KeyCode.Tab;
    private void Awake()
    {        
        UpdateLock();         
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(ConfineKey))
        {
            isConfined=!isConfined;
            UpdateLock();
        }
        
    }
    private void UpdateLock()
    {
        if(isCursorLocked)
        {
            if(isConfined)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }        
    }
}
