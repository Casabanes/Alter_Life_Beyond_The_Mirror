using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField] private Image _manabar;
    public void ActualizeBar(float value)
    {
        _manabar.fillAmount = value;
    }
}
