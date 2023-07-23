using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlancoScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bolea")
        {
            other.GetComponent<BoleaPrefab>().DestroyMe();
        }
    }
}
