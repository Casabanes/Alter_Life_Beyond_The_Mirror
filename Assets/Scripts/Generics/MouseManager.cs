using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    void Start()
    {
        OnOffMouse(true);
    }

    void Update()
    {
        
    }

    public void OnOffMouse(bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        Cursor.visible = !value;

    }
}
