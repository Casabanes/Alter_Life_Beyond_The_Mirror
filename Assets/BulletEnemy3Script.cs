using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BulletEnemy3Script : MonoBehaviour
{
    public float speed;
    public int damage;
    private Action _updateDelegate;
    private void Start()
    {
        StartCoroutine(Die());
        _updateDelegate += Move;
    }

    private void Update()
    {
        _updateDelegate?.Invoke();
    }
    private void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private void Pause(bool value)
    {
        if (value)
        {
            _updateDelegate = delegate { };
            StopCoroutine(Die());
        }
        else
        {
            _updateDelegate += Move;
            StartCoroutine(Die());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        LifeComponent playerLife = other.GetComponent<PlayerLifeComponent>();
        if (playerLife != null)
        {
            playerLife.TakeDamage(damage, new Vector3(0, 0, 0));
        }
        Destroy(gameObject);
    }

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}
