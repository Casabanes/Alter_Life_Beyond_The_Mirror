using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    public GameObject efecto;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LifeComponent>() != null && other.gameObject.layer == 9)
        {
            var boss = other.GetComponent<LifeComponent>();
            boss.TakeDamage(8, boss.transform.position);
            PlayerLifeComponent player = other.GetComponent<PlayerLifeComponent>();
            if (player != null)
            {
                int selectDieWay = Random.Range(0, 2);
                if (selectDieWay == 0)
                {
                    player.dieWay = PlayerLifeComponent.WaysToDie.HitedWithAxe;
                }
                else
                {
                    player.dieWay = PlayerLifeComponent.WaysToDie.Cooked;
                }

            }
        }


        if (other.gameObject.layer != 8)
        {
            DestroyMe();
        }
    }

    public void DestroyMe()
    {
        Instantiate(efecto, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
