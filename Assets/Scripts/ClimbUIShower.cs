using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClimbUIShower : MonoBehaviour
{
    [SerializeField] private Image _interactable;
    [SerializeField] private Image _notInteractable;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _distanceToShowSelf;
    [SerializeField] private float _distanceToShowSelfEntirely;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private int _layerPlayer=9;
    void Start()
    {
        ChangeInteractable();
        GameManager.instance.Changing += ChangeInteractable;
        if (_camera == null)
        {
            _camera = FindObjectOfType<AudioListener>().transform;
            if (_camera == null)
            {
                Destroy(gameObject);
            }
        }
        if(_collider==null)
        {
            _collider = GetComponent<SphereCollider>();
            if (_collider == null)
            {
                Destroy(gameObject);
            }
        }
        if (_distanceToShowSelf < _distanceToShowSelfEntirely)
        {
            _distanceToShowSelf = _distanceToShowSelfEntirely;
        }
        _collider.radius = _distanceToShowSelf;
    }
    private void ChangeInteractable()
    {
        if (GameManager.instance._currentCharacter == null)
        {
            StartCoroutine(TryAgain());
            return;
        }
        switch (GameManager.instance._currentCharacter._character)
        {
            case (BaseCharacter.Identity.jake):
                _interactable.enabled = false;
                _notInteractable.enabled = true;
                break;
            case (BaseCharacter.Identity.Tudor):
                _notInteractable.enabled = false;
                _interactable.enabled = true;
                break;
        }
    }
    private IEnumerator TryAgain()
    {
        yield return new WaitForSeconds(1);
        ChangeInteractable();
    }
    void Update()
    {
        transform.LookAt(_camera.position);
        transform.rotation = Quaternion.Euler(new Vector3(0,transform.rotation.eulerAngles.y, 0));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _layerPlayer)
        {
            ApplyAlpha(Mathf.Lerp(1, 0, (Vector3.Distance(transform.position, GameManager.instance.GetPlayer())
                -3 - _distanceToShowSelfEntirely) / _distanceToShowSelfEntirely));
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == _layerPlayer)
        {
            ApplyAlpha(Mathf.Lerp(1, 0, (Vector3.Distance(transform.position, GameManager.instance.GetPlayer())
                 -3 - _distanceToShowSelfEntirely) / _distanceToShowSelfEntirely));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _layerPlayer)
        {
            ApplyAlpha(0);
        }
    }
    private void ApplyAlpha(float value)
    {
        switch (GameManager.instance._currentCharacter._character)
        {
            case (BaseCharacter.Identity.jake):
                //_notInteractable;
                _notInteractable.color = new Color(_notInteractable.color.r, _notInteractable.color.g, _notInteractable.color.b,value);
                break;
            case (BaseCharacter.Identity.Tudor):
                _interactable.color = new Color(_notInteractable.color.r, _notInteractable.color.g, _notInteractable.color.b,value);
                //_interactable;
                break;
        }
    }
}
