using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testy : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            
            EventManager.instance.Trigger("Curarse");
        }
    }

    
}
