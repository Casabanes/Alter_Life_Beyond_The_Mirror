using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class TutoTramp : MonoBehaviour
{
    [SerializeField] private int _layerPlayer;
    [SerializeField] private Transform _cameraPosition;
    [SerializeField] private float _distanceToShowSelf;
    [SerializeField] private float _distanceToShowSelfEntirely;
    [SerializeField] private Image _pressE;
    [SerializeField] private TutorialManager.Tutorial _tutoType;
    private void Update()
    {
        LookToPlayer();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _layerPlayer)
        {
            _pressE.color = new Color(_pressE.color.r, _pressE.color.g, _pressE.color.b
             , Mathf.Lerp(1, 0, (Vector3.Distance(transform.position, GameManager.instance.GetPlayer())
              - 3 - _distanceToShowSelfEntirely) / _distanceToShowSelfEntirely));
                TutorialManager.instance.InZone(_tutoType,true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == _layerPlayer)
        {
            _pressE.color = new Color(_pressE.color.r, _pressE.color.g, _pressE.color.b
                , Mathf.Lerp(1, 0, (Vector3.Distance(transform.position, GameManager.instance.GetPlayer())
                 - 3 - _distanceToShowSelfEntirely) / _distanceToShowSelfEntirely));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _layerPlayer)
        {
            _pressE.color = new Color(_pressE.color.r, _pressE.color.g, _pressE.color.b,0);
                TutorialManager.instance.InZone(_tutoType,false);
        }
    }
    private void LookToPlayer()
    {
        transform.LookAt(_cameraPosition.position);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
    }
    private void OnDestroy()
    {
            _pressE.color = new Color(_pressE.color.r, _pressE.color.g, _pressE.color.b, 0);
            TutorialManager.instance.InZone(_tutoType, false);
    }
}

