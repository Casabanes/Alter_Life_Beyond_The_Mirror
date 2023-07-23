using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderChange : MonoBehaviour
{
    [SerializeField] private Collider _normalCollider;
    [SerializeField] private Collider _jumpCollider;
    public void Change(bool OnOff)
    {
        if (OnOff)
        {
            _normalCollider.enabled = OnOff;
            _jumpCollider.enabled = !OnOff;
        }
        else
        {
            _normalCollider.enabled = OnOff;
            _jumpCollider.enabled = !OnOff;
        }
    }
}
