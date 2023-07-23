using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : MonoBehaviour
{
    public GameObject Gaucho, boom;
    public Prize prizecosa;
    public LifeScript vid;
    public float speed;
    private Rigidbody rb;
    public AudioSource _audioSource;
    public Animator animator;


    void Start()
    {
        Gaucho = FindObjectOfType<PlayerModel>().gameObject;   
        rb = GetComponent<Rigidbody>(); 
    }

    void FixedUpdate()
    {
        float distance = Vector3.Distance(Gaucho.transform.position, transform.position);

        var lookPos = Gaucho.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;

        if (distance<=20)
        {
            animator.SetBool("InRange", true);
            rb.velocity = transform.forward * speed * Time.deltaTime;
        }

        if (distance <= 1)
        {

            vid.vida.fillAmount -= 0.1f;
            Die();
        }

    }

    public void Die()
    {
        _audioSource.Play();
        Instantiate(boom, transform.position, Quaternion.identity);
        if(prizecosa!=null) prizecosa.objes--;
        Destroy(gameObject);
    }



    //transform.position += transform.forward/12;
    //EventManager.instance.Trigger("PlayerDamage", 15);
}
