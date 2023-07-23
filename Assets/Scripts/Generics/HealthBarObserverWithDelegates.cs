using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarObserverWithDelegates : MonoBehaviour
{
    [SerializeField] private LifeComponent _target;
    [SerializeField] private Image _bar;
    private void Start()
    {
        if (_bar == null)
        {
            _bar = GetComponent<Image>();
        }
        _target.takeDamage += UpdateBar;
    }
    private void UpdateBar(float amount)
    {
        _bar.fillAmount = amount;
    }
}
