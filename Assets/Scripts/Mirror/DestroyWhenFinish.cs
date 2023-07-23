using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenFinish : MonoBehaviour
{
    [SerializeField] private ParticleSystem _ps;
    private void Start()
    {
        if (_ps == null)
        {
            _ps = GetComponent<ParticleSystem>();
        }
    }
    void Update()
    {
        if (!_ps.isPlaying)
        {
            Destroy(gameObject);
        }    
    }
}
