using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _ps;
    void Start()
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
