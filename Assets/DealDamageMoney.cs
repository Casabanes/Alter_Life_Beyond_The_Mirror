using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageMoney : MonoBehaviour
{
    public int damage;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        if (_animator == null)
        {
            _animator = GetComponentInParent<Animator>();
            if (_animator == null)
            {
                Destroy(gameObject);
            }
        }
        EventManager.instance.pause += Pause;
    }
    private void Pause(bool value)
    {
        if (value)
        {
            _animator.speed = 0;
        }
        else
        {
            _animator.speed = 1;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LifeComponent playerLife = other.GetComponent<PlayerLifeComponent>();
            if (playerLife != null)
            {
                playerLife.TakeDamage(damage, new Vector3(0,0,0));

               // Vector3 direction = other.transform.position - transform.position;
               // direction.Normalize();
               // other.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x, direction.y * 2, direction.z) * 1000f, ForceMode.Impulse);
               //no encontré el RB xd.
            }
        }
    }
}
