using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LifeComponent : MonoBehaviour
{
    [SerializeField] protected int _maxLife;
    protected int _actualLife;
    public Animator animator;
    public ParticleSystem particulas;
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected AudioClip[] _damageSound;
    [SerializeField] protected AudioClip[] _deathSound;
    [SerializeField] ParticleSystem[] particulasHeal;
    [SerializeField] protected DeleteParticleAfterPlay _particleSystem;
    [SerializeField] protected DeleteParticleAfterPlay _deathParticleSystem;
    public Action<float> takeDamage=delegate { };
    protected virtual void Start()
    {

        SuscribeToWinLoseManager();
        _actualLife = _maxLife;
    }
    public virtual void TakeDamage(int damage, Vector3 pos)
    {
        _actualLife -= damage;
        if (particulas != null)
        {
            Instantiate(particulas, new Vector3(pos.x, pos.y + 1.5f, pos.z), Quaternion.identity);
        }
        if(_actualLife<=0)
        {
            _actualLife = 0; 
            float value2 = ((float)_actualLife / (float)_maxLife);
            takeDamage(value2);
            Death(pos);
            return;
        }
        if (animator != null)
        {
        animator.SetTrigger("OnDamaged");
        }
        _audioSource.Stop();
        int randomDamageSound = UnityEngine.Random.Range(0, _damageSound.Length);
        _audioSource.clip = _damageSound[randomDamageSound];
        _audioSource.Play();

        float value =((float)_actualLife / (float)_maxLife);
        takeDamage(value);
    }
    public virtual void Heal(int amount)
    {
        foreach (var item in particulasHeal)
        {
            item.Play();
        }
        _actualLife += amount;
        if (_actualLife > _maxLife)
        {
            _actualLife = _maxLife;
        }
        float value = ((float)_actualLife / (float)_maxLife);
            takeDamage(value);
    }
    public virtual void Death(Vector3 pos)
    {
        UnSuscribeToWinLoseManager();
        int randomDeathSound = UnityEngine.Random.Range(0, _deathSound.Length);
        _deathParticleSystem._clip = _deathSound[randomDeathSound];
        DeleteParticleAfterPlay particles = Instantiate(_deathParticleSystem, pos, Quaternion.identity);
        _audioSource.Stop();
        if (particulas != null)
        {
        Instantiate(particulas, new Vector3(pos.x, pos.y + 1.5f, pos.z), Quaternion.identity);
        }
        particles.PlayDaSound(_deathSound[randomDeathSound]);
        Destroy(gameObject);
    }
    public virtual void SuscribeToWinLoseManager()
    {
        WinLoseManager.instance.AddEnemie(gameObject);
    }
    public virtual void UnSuscribeToWinLoseManager()
    {
        WinLoseManager.instance.QuitEnemie(gameObject);
    }
}
