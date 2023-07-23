using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffect : MonoBehaviour
{
    [SerializeField] private AudioSource _as;

    void Start()
    {
        if (_as == null)
        {
            _as = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (!_as.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
