using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeComponent : LifeComponent
{
    private BaseCharacter _character;
    public bool _isDead;
    private Bolea _bolea;

    protected override void Start()
    {
        base.Start();
        _bolea = GetComponent<Bolea>();
    }
    public enum WaysToDie
    {
        Cooked,
        Burned,
        Drowned,
        HitedWithAxe
    }
    public WaysToDie dieWay;
    public override void SuscribeToWinLoseManager()
    {
        WinLoseManager.instance.AddPlayer(gameObject);
    }
    public override void UnSuscribeToWinLoseManager()
    {
        WinLoseManager.instance.QuitPlayer(gameObject, dieWay);
    }
    public override void TakeDamage(int damage, Vector3 pos)
    {
        if (_isDead)
        {
            return;
        }
        base.TakeDamage(damage, pos);
        ActualizeLifeBar();
    }
    public void ActualizeLifeBar()
    {
        EventManager.instance.Trigger("PlayerDamage", (float)_actualLife / (float)_maxLife);
    }
    public override void Death(Vector3 pos)
    {
        _isDead = true;
        _character = GetComponent<BaseCharacter>();
        if (_character)
        {
            _character.GetController().DeathOrRevive(true);
            if (_bolea != null)
            {
                _bolea.enabled = false;
            }
        }
        _audioSource.Stop();
        int randomDeathSound = UnityEngine.Random.Range(0, _deathSound.Length);
        _audioSource.clip = _deathSound[randomDeathSound];
        _audioSource.Play();
        animator.SetTrigger("Death");
        GameManager.instance.YouDie();
        //UnSuscribeToWinLoseManager();
        this.enabled = false;
    }
    private void Update()
    {
    }
    public void OutOfMapHeal(int amount)
    {
        _actualLife += amount;
        if (_actualLife >= _maxLife)
        {
            _actualLife = _maxLife;
        }
    }
    public override void Heal(int amount)
    {
        if (!_character)
        {
            _character = GetComponent<PlayerCharacter>();
            if (_character)
            {
                _character.GetController().DeathOrRevive(false);
            }
        }
        base.Heal(amount);
        float heal = ((float)_actualLife / (float)_maxLife);
        EventManager.instance.Trigger("PlayerDamage", heal);
        _isDead = false;
        _audioSource.Stop();
        int randomDamageSound = UnityEngine.Random.Range(0, _damageSound.Length);
        _audioSource.clip = _damageSound[randomDamageSound];
        _audioSource.Play();
        _bolea.enabled = true;
    }
}
