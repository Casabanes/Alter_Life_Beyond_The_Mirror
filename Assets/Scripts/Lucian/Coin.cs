using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coins;
    public AudioSource soundy;
    public float RotateSpeed;
    public GameObject Effecty;

    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, RotateSpeed, 0, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            soundy.Play();
            Instantiate(Effecty, transform.position, Quaternion.identity);
            print("Moneda Destruida");
            Destroy(gameObject);
        }
    }
}
