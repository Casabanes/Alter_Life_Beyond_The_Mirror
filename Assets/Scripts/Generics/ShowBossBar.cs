using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBossBar : MonoBehaviour
{
    [SerializeField] private Image[] _bossBar;
    [SerializeField] private Transform _player;
    [SerializeField] private float _range;

    private void Start()
    {
    }
    private void FixedUpdate()
    {
        if (Vector3.Distance(_player.position,transform.position)<= _range)
        {
            foreach (var item in _bossBar)
            {
                item.enabled = true;
            }
        }
        else
        {
            foreach (var item in _bossBar)
            {
                item.enabled = false;
            }
        }
    }
}
