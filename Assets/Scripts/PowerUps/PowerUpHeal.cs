using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PowerUpHeal : MonoBehaviour
{
    [SerializeField] private LayerMask _layerFloor;
    [SerializeField] private int _layerPlayer;
    [SerializeField] private int _healAmount;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _timeToDesapear=0.5f;
    [SerializeField] private Collider _collider;
    private float _currentTime;
    private Vector3 _originalScale;
    private Vector3 _originalPosition;

    private Action disapear=delegate { };
    private void Start()
    {
        _originalScale = transform.localScale;
        _currentTime=_timeToDesapear;
        if (_collider == null)
        {
            _collider = GetComponent<Collider>();
        }
    }
    private void Update()
    {
        FloatingEffect();
        RotatingEffect();
        disapear();
    }
    public void RotatingEffect()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
    }
    public void FloatingEffect()
    {
        Vector3 direction = Vector3.up * (0.01f * Mathf.Cos(Time.time * 5));
        transform.position += (Vector3.up * direction.y);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _layerPlayer)
        {
            LifeComponent lc = other.gameObject.GetComponent<LifeComponent>();
            lc.Heal(_healAmount);
            disapear = Disapear;
            _collider.enabled = false;
        }
    }
    private void Disapear()
    {
        float realTimeToDisapear = _currentTime/ _timeToDesapear;
        transform.localScale = _originalScale * realTimeToDisapear;
        _currentTime -= Time.deltaTime;
        if (transform.localScale.magnitude < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
