using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoleaPrefab : MonoBehaviour
{
    public GameObject efecto;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LifeComponent>() != null)
        {
            var boss = other.GetComponent<LifeComponent>();
            boss.TakeDamage(8 ,boss.transform.position);

        }

        if (other.GetComponent<Enemy_1>() != null)
        {
            var enem = other.GetComponent<Enemy_1>();
            enem.Die();

        }


        DestroyMe();
    }

    public void DestroyMe()
    {
        Instantiate(efecto, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
