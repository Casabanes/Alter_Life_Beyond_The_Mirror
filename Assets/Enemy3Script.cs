using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy3Script : MonoBehaviour
{
    public Action Estado;
    public Action _previousState;
    public int Direc;
    public float TimeToCHange, TimeToShoot, rotationSpeed, HastaDondeTeSigue, Speed;
    public Transform targetTransform, ShootDirection;
    public SphereCollider sc;
    public GameObject Bullet;
    private Transform InitialTransform;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _alertSource;
    [SerializeField] private GameObject _spotedMesh;
    void Start()
    {
        InitialTransform = transform;
        Direc = 1;
        Estado = Idle;
        StartCoroutine(ChangeSide());
        EventManager.instance.pause += Pause;
    }

    void Update()
    {
        Estado?.Invoke();
    }
    private void Pause(bool value)
    {
        if (value)
        {
            _previousState = Estado;
            Estado = delegate { };
            StopAllCoroutines();
        }
        else
        {
            Estado = _previousState;
        }
    }
    public void Idle()
    {
        transform.Rotate(Vector3.up, Direc * Speed);
    }

    public IEnumerator ChangeSide()
    {

        yield return new WaitForSeconds(TimeToCHange);
        Direc = Direc * -1;
        StartCoroutine(ChangeSide());
    }

    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(TimeToShoot);
        _audioSource.Stop();
        _audioSource.Play();
        Instantiate(Bullet, ShootDirection.position, Quaternion.LookRotation(targetTransform.position - transform.position));
        StartCoroutine(Shoot());
    }

    public void Follow()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - targetTransform.position);
        float targetYRotation = targetRotation.eulerAngles.y;

        float currentYRotation = transform.rotation.eulerAngles.y;
        float newYRotation = Mathf.LerpAngle(currentYRotation, targetYRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, newYRotation, 0f);

        float distance = Vector3.Distance(transform.position, targetTransform.position);

        if(distance > 40f) 
        {
            
            transform.rotation = InitialTransform.rotation;
            Estado = Idle;
            StopAllCoroutines();
            _spotedMesh.SetActive(false);
            StartCoroutine(ChangeSide()); 
            sc.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _alertSource.Play();
        _spotedMesh.SetActive(true);
        sc.enabled = false;
        targetTransform = other.gameObject.transform;
        Estado = Follow;
        StopCoroutine(ChangeSide());
        StartCoroutine(Shoot());
    }
    private void CheckTarget()
    {
        if (targetTransform != null && !targetTransform.gameObject.activeSelf)
        {
            transform.rotation = InitialTransform.rotation;
            Estado = Idle;
            StopAllCoroutines();
            _spotedMesh.SetActive(false);
            StartCoroutine(ChangeSide());
            sc.enabled = true;
        }
    }
}
