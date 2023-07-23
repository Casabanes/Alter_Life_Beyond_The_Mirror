using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpMirror : MonoBehaviour
{
    [SerializeField] private int _layerCharacter;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==_layerCharacter)
        {
            if (other.gameObject.GetComponent<PlayerCharacter>())
            {
                other.gameObject.GetComponent<PlayerCharacter>().GetPlayerController().GetMirror();
                GameManager.instance.mirror.StartMoving();
                Destroy(gameObject);
            }
        }
    }
}
