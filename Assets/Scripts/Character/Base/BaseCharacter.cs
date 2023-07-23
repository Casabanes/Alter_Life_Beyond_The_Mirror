using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    public enum Identity
    {
        jake,
        Tudor
    };
    protected BaseController _controller;
    [SerializeField] public Identity _character;
    [SerializeField] protected Identity _characterToChange;
    public virtual void Initialize(Transform t) { }
    public virtual void ChangeCharacter() { }
    [SerializeField] protected Transform _mirrorTarget;
    [SerializeField] protected Transform _mirrorSecondaryTarget;

    [SerializeField] protected AudioClip[] _damageClip;
    [SerializeField] protected AudioClip[] _deathClip;
    [SerializeField] protected BaseModel _model;
    protected virtual void Start()
    {
        if (_model == null)
        {
            _model = GetComponent<BaseModel>();
            if (_model == null)
            {
                Debug.Log("model not found");
                gameObject.SetActive(false);
            }
        }
    }
    public Transform GetMirrorTarget()
    {
        return _mirrorTarget;
    }
    public Transform GetMirrorSecondaryTarget()
    {
        return _mirrorSecondaryTarget;
    }
    public virtual BaseController GetController() 
    {
        if (_controller == null)
        {
            switch (_character)
            {
                case (Identity.jake):
                _controller = new PlayerController(_model);
                    break;
                case (Identity.Tudor):
                    _controller = new TudorController(_model);
                    break;
            }
        }
        return _controller;
    }
    public virtual void OnOffControls(bool value) { }

}
