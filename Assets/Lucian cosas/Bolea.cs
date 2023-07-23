using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolea : MonoBehaviour
{
    bool puedetirar;
    public Animator playerAnim;
    [SerializeField] private float _forwardMultiplier;
    [SerializeField] private float _upwardMultiplier;

    private void Start()
    {
        

        puedetirar = true;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartCoroutine(SpawnBolea());
           // TirarBolea();
        }
    }

    

    IEnumerator SpawnBolea()
    {
        if (puedetirar == true)
        {
            playerAnim.SetTrigger("_isThrowing");
            puedetirar = false;
            TirarBolea();
            yield return new WaitForSeconds(1f);
            puedetirar = true;

        }
    }


    void TirarBolea()
    {
        var bolea = Resources.Load("BoleaPrefab") as GameObject;
        var disparo = Instantiate(bolea, transform.position + (transform.up * 1.9f + transform.right * 0.8f), Quaternion.identity);
        var gaucho = GameObject.Find("Camera");
        Rigidbody rb = disparo.GetComponent<Rigidbody>();
        Vector3 newdirection = gaucho.transform.forward;
        newdirection.y = 0;
        gameObject.transform.forward = newdirection;
        rb.AddForce(gaucho.transform.forward * _forwardMultiplier + transform.up * _upwardMultiplier, ForceMode.Impulse);
        rb.AddTorque(disparo.transform.up * 500f);
    }

}
