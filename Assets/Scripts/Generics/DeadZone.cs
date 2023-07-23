using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
     [SerializeField] private AutoCheckPoint _autoCheckPoint;
     public AudioSource splash;
     GameObject cuby;


    private void Start()
    {
        var cubo = Resources.Load("BlackCube") as GameObject;
        var camera = GameObject.Find("Camera");
        var scenecube = Instantiate(cubo, camera.transform.position + (transform.forward * 1.1f), Quaternion.identity);
        scenecube.transform.parent = camera.transform;
        scenecube.SetActive(false);
        cuby = scenecube;

        if (_autoCheckPoint == null)
        {
            _autoCheckPoint= FindObjectOfType<AutoCheckPoint>();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerModel>())
        {
            cuby.SetActive(true);
            splash.Play();
            _autoCheckPoint.SoundSplash();

            //var cosa = scenecube.GetComponent<MeshRenderer>().material.color.a;
            //cosa = Mathf.Clamp(cosa, 0, 1);

            StartCoroutine(Spawn());
  
            //collision.transform.position = _autoCheckPoint.transform.position;
        }
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1f);
        var gaucho = GameObject.Find("Gauchito-Kun");
        gaucho.transform.position = _autoCheckPoint.transform.position;
        cuby.SetActive(false);
    }
}
