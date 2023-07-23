using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WinLoseManager : MonoBehaviour
{
    [SerializeField] private string _loseScene;
    [SerializeField] private string _winScene;

    [SerializeField] private List<GameObject> _enemies=new List<GameObject>();
    [SerializeField] private GameObject _player;
    public static WinLoseManager instance;
    private const int _constZero = 0;
    [SerializeField] MouseManager _mousemanager;
    [SerializeField] private bool _winWhenKillAllEnemies;
    [SerializeField] private Transform _deathCameraCookedPos;
    [SerializeField] private Transform _deathCameraHittedWithAxePos;
    [SerializeField] private Transform _deathCameraBurnedPos;


    [SerializeField] private Transform _camera;

    [SerializeField] private Animator[] loseAnimators;
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private LifeComponent playerLifeComponent;
    [SerializeField] private PlayerCharacter _controlAcces;

    [SerializeField] private AudioSource _musicOfLevel;
    [SerializeField] private AudioClip _payadaMusic;
    [SerializeField] private AudioClip _himnomusic;
    [SerializeField] private AudioClip _defeatMusic;
    private PlayerLifeComponent.WaysToDie _waysToDie;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        if (_player == null)
        {
            _player = FindObjectOfType<PlayerModel>().gameObject;
        }
    }
    public IEnumerator Win()
    {
        _mousemanager.OnOffMouse(false);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(_winScene);
    }
    public IEnumerator Lose()
    {
        _musicOfLevel.Stop();
        _mousemanager.OnOffMouse(false);
        foreach (var item in loseAnimators)
        {
            item.SetBool("OnOff", false);
        }
        yield return new WaitForSeconds(1f);
        fadeAnimator.SetTrigger("Go");
        yield return new WaitForSeconds(1.15f);
        _musicOfLevel.clip = _defeatMusic;
        _musicOfLevel.volume = 0.1f;
        _musicOfLevel.Play();
        switch (_waysToDie)
        {
            case PlayerLifeComponent.WaysToDie.Burned:
                _camera.position = _deathCameraBurnedPos.position;
                _camera.rotation = _deathCameraBurnedPos.rotation;

                break;
            case PlayerLifeComponent.WaysToDie.Cooked:
                _camera.position = _deathCameraCookedPos.position;
                _camera.rotation = _deathCameraCookedPos.rotation;
                break;
            case PlayerLifeComponent.WaysToDie.Drowned:
                break;
            case PlayerLifeComponent.WaysToDie.HitedWithAxe:
                _camera.position = _deathCameraHittedWithAxePos.position;
                _camera.rotation = _deathCameraHittedWithAxePos.rotation;
                _camera.parent = _deathCameraHittedWithAxePos;
                break;
        }
       
    }
    public void Restart()
    {

        _musicOfLevel.Stop();
        _musicOfLevel.loop = false;
        _musicOfLevel.clip = _himnomusic;
        _musicOfLevel.Play();
        _mousemanager.OnOffMouse(true);
        fadeAnimator.SetTrigger("Go");
        StartCoroutine(Restarting());
    }
    /*
     * 
     * 
     *   playerLifeComponent.enabled = true;
        playerAnimator.SetTrigger("Raise");
        playerLifeComponent.Heal(1000);
     * */
    private IEnumerator Restarting()
    {
        yield return new WaitForSeconds(1f);
        _camera.transform.parent = _camera.transform;
        playerLifeComponent.enabled = true;
        playerAnimator.SetTrigger("Raise");
        foreach (var item in loseAnimators)
        {
            item.SetBool("OnOff", true);
        }
        yield return new WaitForSeconds(6.11f);
        _controlAcces.GetController().DeathOrRevive(false);
        playerLifeComponent.Heal(1000);
        _musicOfLevel.Stop();
        _musicOfLevel.loop = true;
        _musicOfLevel.volume = 0.1f;
        _musicOfLevel.clip = _payadaMusic;
        _musicOfLevel.Play();
        AddPlayer(_controlAcces.gameObject);
    }
    public void AddPlayer(GameObject player) 
    {
        _player = player;
    }
    public void AddEnemie(GameObject enemie)
    {
        if (!_enemies.Contains(enemie))
        {
            _enemies.Add(enemie);
        }
    }
    public void QuitPlayer(GameObject player, PlayerLifeComponent.WaysToDie waysToDie)
    {
        _controlAcces.GetController().DeathOrRevive(true);
        _waysToDie = waysToDie;
        if (_player)
        {
            StartCoroutine(Lose());
            _player = null;
        }
    }
    public void QuitEnemie(GameObject enemie)
    {
        if (_enemies.Contains(enemie))
        {
            _enemies.Remove(enemie);
        }
        if(_enemies.Count== _constZero && _winWhenKillAllEnemies)
        {
            StartCoroutine(Win());
        }
    }
}
