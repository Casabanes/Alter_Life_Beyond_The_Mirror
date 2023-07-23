using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAdvisory : MonoBehaviour
{
    [SerializeField] private int _layerToNotify;
    [SerializeField] private JumpingFoot _jumpingFoot;
    private int _touchingFloors;
    private const int _constZero = 0;

    private void Start()
    {
        if (_jumpingFoot == null)
        {
            _jumpingFoot = transform.parent.gameObject.GetComponent<JumpingFoot>();
            if (_jumpingFoot == null)
            {
                Debug.Log("JumpingFoot not foun");
                gameObject.SetActive(false);
            }
        }
    }
    public TriggerAdvisory SetJumpingFoot(JumpingFoot jumpingFoot)
    {
        _jumpingFoot = jumpingFoot;
        return this;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _layerToNotify)
        {
            _touchingFloors++;
            _jumpingFoot.IsGrounded(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _layerToNotify)
        {
            _touchingFloors--;
            if (_touchingFloors == _constZero)
            {
                _jumpingFoot.IsGrounded(false);
            }
        }
    }
}
