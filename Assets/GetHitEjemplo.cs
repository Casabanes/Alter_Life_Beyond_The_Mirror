using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHitEjemplo : MonoBehaviour
{
    //Este Script solo es a modo de ejemplo!!
    public GameObject boom;


    public void BoomBoom()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Instantiate(boom, transform.position + transform.up * 1.2f, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
