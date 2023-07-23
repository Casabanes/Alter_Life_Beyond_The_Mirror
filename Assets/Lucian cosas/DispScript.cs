using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class DispScript : MonoBehaviour
{
    public GameObject[] Cubito;

    private void Start()
    {   
        foreach (var c in Cubito)
        {
            c.gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BoleaPrefab>() != null)
        {
            foreach (var c in Cubito)
            {
                c.gameObject.SetActive(true);
            }

            Destroy(gameObject);
        }

    }

}
