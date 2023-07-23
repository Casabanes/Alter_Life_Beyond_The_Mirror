using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : MonoBehaviour
{
    public GameObject Gaucho, boom;
    public LifeScript vid;
    public float speed;
    private Rigidbody rb;
    public AudioSource _audioSource;
    [SerializeField] private float _forwardMultiplier;
    [SerializeField] private float _upwardMultiplier;
    [SerializeField] private GameObject _axe;
    private bool _canshoot;
    public Prize prizecosa;
    public float Rangee, Miny, Maxy;
    public Animator animator;

    void Start()
    {
        _canshoot = true;
        rb = GetComponent<Rigidbody>();
        if (Gaucho == null)
        {
            Gaucho = FindObjectOfType<PlayerModel>().gameObject;
        }
    }

    void FixedUpdate()
    {
        float distance = Vector3.Distance(Gaucho.transform.position, transform.position);

        var lookPos = Gaucho.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;

        if (distance <= Rangee)
        {
            if (_canshoot == true)
            {
                StartCoroutine(Shot());
            }
        }

        if (distance <= 1)
        {
            vid.vida.fillAmount -= 0.1f;
        }
    }

    public void Die()
    {
        _audioSource.Play();
        Instantiate(boom, transform.position, Quaternion.identity);
        if (prizecosa != null) prizecosa.objes--;
        Destroy(gameObject);
    }

    public void Shoot()
    {
        var axe = Instantiate(_axe);
        var disparo = Instantiate(axe, gameObject.transform.position + (transform.up * 1.4f), Quaternion.identity);
        disparo.transform.forward = transform.forward;
        Rigidbody rb = disparo.GetComponent<Rigidbody>();
        //rb.AddForce(transform.forward * _forwardMultiplier + transform.up * _upwardMultiplier, ForceMode.Impulse);
        rb.AddForce(Gaucho.transform.position - transform.position + transform.up * _upwardMultiplier, ForceMode.Impulse);
        rb.AddTorque(disparo.transform.right * 800f);
        _canshoot = false;
    }

    IEnumerator Shot()
    {

        Shoot();
        animator.SetTrigger("AttackTriggered");
        float tiemp = Random.Range(Miny, Maxy);
        yield return new WaitForSeconds(tiemp);
        _canshoot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Die();
        }

    }


    //transform.position += transform.forward/12;
    //EventManager.instance.Trigger("PlayerDamage", 15);
}
