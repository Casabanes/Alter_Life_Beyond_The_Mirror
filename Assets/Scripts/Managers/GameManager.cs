using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private static BaseModel player;
    [SerializeField] private static PlayerLifeComponent playerLife;
    [SerializeField] private BaseCharacter.Identity _startingCharacter;
    [SerializeField] public BaseCharacter _currentCharacter;

    [SerializeField] private BaseCharacter[] _inGameCharacters;
    [SerializeField] public Dictionary<BaseCharacter.Identity, BaseCharacter> _characters
        =new Dictionary<BaseCharacter.Identity, BaseCharacter>();
    [SerializeField] private Cinemachine.CinemachineFreeLook _camera;
    [SerializeField] private FollowObject _cameraReference;
    [SerializeField] public Mirror mirror;
    [SerializeField] private GameObject _youDie;
    [SerializeField] public ManaBar manaBar;
    public Action Changing;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        foreach (var item in _inGameCharacters)
        {
            _characters.Add(item._character, item);
        }
        if (_currentCharacter == null)
        {
            _currentCharacter = _characters[_startingCharacter];
        }
        if (player == null)
        {
            player = _currentCharacter.gameObject.GetComponent<BaseModel>();
        }
        if (playerLife == null)
        {
            player = _currentCharacter.gameObject.GetComponent<BaseModel>();
        }
        if (_camera == null)
        {
            _camera = FindObjectOfType<Cinemachine.CinemachineFreeLook>();
        }
            if (_cameraReference == null)
        {
            _cameraReference = FindObjectOfType<FollowObject>();
        }
        if (mirror == null)
        {
            mirror = FindObjectOfType<Mirror>();
        }
        StartCoroutine(DisableOtherCharacters());
        EventManager.instance.pause += ActualizeLifeBar;
    }
    private IEnumerator DisableOtherCharacters()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (var item in _characters)
        {
            if (item.Key != _startingCharacter)
            {
                item.Value.gameObject.SetActive(false);
            }
        }
    }
    void Update()
    {

    }
    public Vector3 GetPlayer()
    {
        return _currentCharacter.transform.position;
    }
    public bool GetPlayerIsDead()
    {
        if (playerLife == null)
        {
            if (player == null)
            {
                player = _characters[_startingCharacter].gameObject.GetComponent<BaseModel>();
            }
            playerLife = player.gameObject.GetComponent<PlayerLifeComponent>();
        }
        return playerLife._isDead;
    }

    public void YouDie()
    {
        _youDie.gameObject.SetActive(true);
    }
    public void ActualizeLifeBar(bool value)
    {
        if(value)
        {
            if (playerLife != null)
            {
                playerLife.ActualizeLifeBar();
            }
        }
    }

    public void ChangeCharacters(BaseCharacter.Identity to,Transform t)
    {
        _characters[to].gameObject.SetActive(true);
        _characters[to].Initialize(t);
        playerLife = _characters[to].GetComponent<PlayerLifeComponent>();
        playerLife.ActualizeLifeBar();
        _camera.Follow = _characters[to].gameObject.transform;
        _camera.LookAt = _characters[to].gameObject.transform;
        _cameraReference.SetTarget(_characters[to].gameObject.transform);
        mirror.SetTarget(_characters[to].GetMirrorTarget(), _characters[to].GetMirrorSecondaryTarget());
        mirror.SetCharacter(_characters[to].gameObject.transform,to);
        _currentCharacter = _characters[to];
        mirror.BreakMirror();
        Changing?.Invoke();
    }
    public GameObject GetPlayerObjetct()
    {
        return _currentCharacter.gameObject;
    }
}
