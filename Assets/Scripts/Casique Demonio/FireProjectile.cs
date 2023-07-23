using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _distanceToExplode;
    [SerializeField] private int _damage;
    [SerializeField] private FloorFire _fireFloor;
    [SerializeField] private LayerMask _placesToInstantiateFloorFire;
    public ParticleSystem particulasDissolve;
    [SerializeField] private Vector3 _endPoint;
    private DifuntaCorreaAgent _owner;
    private float _currentDistance;

    public void SetOwner(DifuntaCorreaAgent owner)
    {
        _owner = owner;
    }
    public void SetDamage(int newDamage)
    {
        _damage = newDamage;
    }
    private void Start()
    {
        _endPoint=CalculateEnd();
    }
    void Update()
    {
        _currentDistance += Vector3.Magnitude(transform.forward * _speed * Time.deltaTime);
        transform.position += transform.forward * _speed * Time.deltaTime;
        if (_currentDistance >= _distanceToExplode)
        {
            Explode();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _owner.gameObject)
        {
            return;
        }
        LifeComponent target = other.GetComponent<LifeComponent>();
        if (target!=null)
        {
            target.TakeDamage(_damage,transform.position);
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
    public Vector3 CalculateEnd()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        if (!Physics.Raycast(origin
            , direction, out hit, _distanceToExplode))
        {
            origin = transform.position + transform.forward * _distanceToExplode;
            direction = -Vector3.up;
            Physics.Raycast(origin
            , direction, out hit, Mathf.Infinity, _placesToInstantiateFloorFire);
        }
        else
        {
        }
        return hit.point;
    }
    public void Explode()
    {
        FloorFire aux = Instantiate(_fireFloor, _endPoint, Quaternion.identity);
        aux.SetOwner(_owner);
        aux.SetDamage(_damage);
        Instantiate(particulasDissolve, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
