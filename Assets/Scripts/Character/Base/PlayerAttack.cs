using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _weaponObject;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private PlayerMovement _movement;
    public Action<bool> isAttacking;
    [SerializeField] private float _attackCD;
    [SerializeField] private float _attackDelay = 0.2f;
    [SerializeField] private int[] _hittables;
    private bool _isAttacking;
    private void Start()
    {
        if (_weaponObject != null)
        {
            _weapon = _weaponObject.GetComponent<Weapon>();
        }
    }

    public void Attack() //empieza el ataque
    {
        if (_isAttacking)
        {
            return;
        }
        _isAttacking = true;
        isAttacking?.Invoke(true);
        StartCoroutine(WaitOnOff());
    }
    private IEnumerator WaitOnOff()
    {
        yield return new WaitForSeconds(_attackDelay);
        CheckAndDoDamage();
            StartCoroutine(AttackColdown());
    }
    private void CheckAndDoDamage()
    {
        Collider[] gos = Physics.OverlapBox(transform.position + Vector3.up * 2, new Vector3(0.25f, 1, 3.5f), transform.rotation);
        foreach (var item in gos)
        {
            for (int i = 0; i < _hittables.Length; i++)
            {
                if (item.gameObject.layer == _hittables[i])
                {
                    if (Vector3.Dot(transform.position, item.gameObject.transform.position) > 0)
                    {
                        GameObject hitted = item.gameObject;
                        LifeComponent target = hitted.GetComponent<LifeComponent>();
                        IHittable hittable = hitted.GetComponent<IHittable>();
                        if (target != null)
                        {
                            target.TakeDamage(_weapon.Damage(), item.ClosestPoint(transform.position));
                        }
                        if (hittable != null)
                        {
                            hittable.GetHit();
                        }
                    }
                }
            }
        }
    }
    private IEnumerator AttackColdown()
    {
        yield return new WaitForSeconds(_attackCD);
        _isAttacking = false;
            isAttacking?.Invoke(false);
    }
    public void TurnOnWeaponObject()
    {
        _weaponObject.SetActive(true);
    }
}
