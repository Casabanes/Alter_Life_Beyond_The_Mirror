using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    void Start()
    { 
        EventManager.instance.Suscribe("Curarse", Curarse);
    }

    private void Curarse(params object[] parameters)
    {
        print("Soy " + this.gameObject.name + "y me curé.");
    }
}
