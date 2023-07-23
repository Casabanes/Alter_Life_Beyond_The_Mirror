using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootLauncherParticles : MonoBehaviour
{
    [SerializeField] private int _layerFloor=6;
    [SerializeField] private float _minScale;
    [SerializeField] private float _maxScale;
    private Quaternion rotation;
    [SerializeField] private ParticleSystem _psGrass;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == _layerFloor)
        {
            rotation = Quaternion.Euler(Vector3.up *Random.Range(0,360));
            _psGrass.transform.localScale = Vector3.one * (Random.Range(_minScale, _maxScale));
            Instantiate(_psGrass, transform.position, rotation);
        }
    }
}
