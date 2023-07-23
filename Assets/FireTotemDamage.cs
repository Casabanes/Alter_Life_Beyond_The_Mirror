using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTotemDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float _timePerTick;
    [SerializeField] private AudioEffect _burnEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LifeComponent playerLife = other.GetComponent<PlayerLifeComponent>();
            if (playerLife != null)
            {
                Instantiate(_burnEffect, other.transform);
                playerLife.TakeDamage(damage, new Vector3(0, 0, 0));
            }
        }
    }
}
