using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBreak : MonoBehaviour, IHittable
{
    
    [SerializeField] private float _random1Y, _random2Y, _random1X, _random2X;
    public GameObject boom;
    public int _cantidad;

    public void BreakMirror()
    {
        for (int i = 0; i < _cantidad; i++)
        {
            var espejos = Resources.Load("Mirror Piece") as GameObject;
            var disparo = Instantiate(espejos, transform.position + transform.up * 1.2f, Quaternion.Euler(0f, Random.Range(0, 360), 0f));
            MirrorPiece mp = disparo.GetComponent<MirrorPiece>();
            //mp.DisableColl(gameObject);
            Rigidbody rb = disparo.GetComponent<Rigidbody>();
            rb.AddForce(disparo.transform.forward * Random.Range(_random1X, _random2X) + transform.up * Random.Range(_random1X, _random2X), ForceMode.Impulse);
            rb.AddTorque(disparo.transform.up * Random.Range(1f, 3f));

            Instantiate(boom, transform.position + transform.up * 1.2f, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    public void GetHit()
    {
        BreakMirror();
    }
}
