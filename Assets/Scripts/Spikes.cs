using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private int _layerPlayer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _layerPlayer)
        {
            PlayerLifeComponent target = other.GetComponent<PlayerLifeComponent>();
            target.TakeDamage(_damage,transform.position);
        }
    }
}
