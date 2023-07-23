using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [SerializeField] private string _level;
    public void PlayAgin()
    {
        WinLoseManager.instance.Restart();
    }

}
