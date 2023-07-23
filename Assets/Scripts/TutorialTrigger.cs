using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    private int _layerPlayer = 9;
    [SerializeField] private TutorialManager.Tutorial _tutorialType;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _layerPlayer)
        {
            TutorialManager.instance.CollisionWithPlayer(_tutorialType);
            Destroy(gameObject);
        }
    }
}
