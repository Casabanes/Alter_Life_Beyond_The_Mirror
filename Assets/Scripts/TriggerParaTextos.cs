using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerParaTextos : MonoBehaviour
{
    [SerializeField] private GameObject _text;
    [SerializeField] private float _timeToDisapear;
    [SerializeField] private int _layerPlayer;
    private void Start()
    {
        if (_text == null)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _layerPlayer)
        {
            _text.SetActive(true);
            StartCoroutine(TurnOffObject());
            GetComponent<Collider>().enabled = false;
        }
    }
    private IEnumerator TurnOffObject()
    {
        yield return new WaitForSeconds(_timeToDisapear);
        _text.SetActive(false);
        Destroy(gameObject);
    }
}
