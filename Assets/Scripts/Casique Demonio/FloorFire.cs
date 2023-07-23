using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFire : MonoBehaviour
{
    [SerializeField] private int _damage;
    private FloorFire _brother;
    private DifuntaCorreaAgent _owner;
    public ParticleSystem particulasDissolve;
    public float TimeToDie;

    private void Start()
    {
        TimeToDie = 10;
        StartCoroutine(FireOff());
    }
    public void SetOwner(DifuntaCorreaAgent owner)
    {
        _owner = owner;
    }
    public void SetDamage(int newDamage)
    {
        _damage = newDamage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_owner != null)
        {
            if (other.gameObject == _owner.gameObject)
            {
                return;
            }
        }
        _brother = other.GetComponent<FloorFire>();
        if (_brother != null)
        {
            //aca llamo al manager
        }
        LifeComponent target = other.GetComponent<LifeComponent>();
        if (target != null)
        {
            target.TakeDamage(_damage, transform.position);
            Instantiate(particulasDissolve, transform.position, Quaternion.identity);
            return;
        }
        PlayerLifeComponent player = other.GetComponent<PlayerLifeComponent>();
        if (player != null)
        {
            player.dieWay = PlayerLifeComponent.WaysToDie.Burned;
        }
        Explode();
    }
    public void Explode()
    {
        //feedback
        Destroy(gameObject);
    }
    private IEnumerator FireOff()
    {
        yield return new WaitForSeconds(TimeToDie);
        Instantiate(particulasDissolve, transform.position, Quaternion.identity);
        Explode();
    }
}
