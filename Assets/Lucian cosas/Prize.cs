using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour
{
    public int objes;
    public GameObject cofre, boom;
    public AudioSource _audioSource, _audioSource2;

    private void FixedUpdate()// re cabeza
    {
        if(objes == 0)
        {
            
            Invoke("Appear", 0.5f);
        }





    }


    public void Appear()
    {
        Instantiate(cofre, transform.position, Quaternion.identity);

        Instantiate(boom, transform.position, Quaternion.identity);
        Instantiate(boom, transform.position, Quaternion.identity);
        Instantiate(boom, transform.position, Quaternion.identity);
        _audioSource.Play();
        _audioSource2.Play();
        Destroy(gameObject);
    }
}
