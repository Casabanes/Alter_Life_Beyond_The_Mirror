using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFacon : MonoBehaviour
{
    [SerializeField] private int _layerCharacter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _layerCharacter)
        {
            if (other.gameObject.GetComponent<PlayerModel>())
            {
                other.gameObject.GetComponent<PlayerModel>().FindignWeapon(transform.position);
                Destroy(gameObject);
            }
        }
    }
}
