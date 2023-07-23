using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public enum Tutorial
    {
        Movement,
        Dash,
        Attack,
        MirrorBroken,
        MirrorInstructions,
        DoubleJump,
        Climb,
        JakeInfo,
        TudorInfo,
        Pilar,
        PressToAccesInfo,
        FireThrower,
        Darts,
        Axe,
        None
    };
    public Tutorial _state=Tutorial.Movement;
    public GameObject _tutoBaseControls;
    public GameObject _tutoDash;
    public GameObject _tutoAttack;
    public GameObject _tutoMirrorBroken;
    public GameObject _tutoMirror;
    public GameObject _tutoDobleJump;
    public GameObject _tutoClimb;
    public GameObject _tutoTudor;
    public GameObject _tutoJake;
    public GameObject _tutoPilar;
    public GameObject _win;
    public GameObject _mirrorPiece;
    public CharacterInfo _pressToAccesInfo;
    public GameObject _pressToAccesInfo2;
    public GameObject _trapInfoFireThrower;
    public GameObject _trapInfoDarts;
    public GameObject _trapInfoAxe;
    public GameObject[] _lifeBar;
    public bool[] _lifeBarState;

    private bool _isWaiting;
    public BaseCharacter _p;
    private int _tutoPlayer = 2;
    public static TutorialManager instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        _state = 0;
        _tutoBaseControls.SetActive(true);
        EventManager.instance.TutorialPauseGame(true);
        EventManager.instance.pause+=EnableAndDisableLifebar;
        GameManager.instance.Changing += TutoPlayer;
        GameManager.instance.Changing += ChangeLifeBar;

        _lifeBarState = new bool[_lifeBar.Length];
        for (int i = 0; i < _lifeBar.Length; i++)
        {
            _lifeBarState[i] = _lifeBar[i].activeSelf;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            switch(_state)
            {
                case (Tutorial.Movement):
                    _tutoBaseControls.SetActive(false);
                    if (GameManager.instance._currentCharacter == _p)
                    {
                        _p.gameObject.SetActive(true);
                        EventManager.instance.TutorialPauseGame(false);
                    }
                    break;
            }
        }
        if (!_pressToAccesInfo.enabled)
        {
            if (_tutoJake.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    _tutoJake.SetActive(false);
                    EventManager.instance.TutorialPauseGame(false);
                    StartCoroutine(ActualizeLifeBar());
                }
            }
            if (_tutoTudor.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    _tutoTudor.SetActive(false);
                    EventManager.instance.TutorialPauseGame(false);
                    StartCoroutine(ActualizeLifeBar());
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!_isWaiting)
            {
                if (inZone)
                {
                    _state = zone;
                }
                switch (_state)
                {
                    case (Tutorial.Dash):
                        if (_tutoDash.activeSelf)
                        {
                            _tutoDash.SetActive(false);
                            EventManager.instance.TutorialPauseGame(false);
                            EnableAndDisableLifebar(false);
                        }
                        // GameManager.instance._currentCharacter.OnOffControls(true);
                        break;
                    case (Tutorial.Attack):
                        if (_tutoAttack.activeSelf)
                        {
                            _tutoAttack.SetActive(false);
                            EnableAndDisableLifebar(false);
                            EventManager.instance.TutorialPauseGame(false);
                        }

                        //GameManager.instance._currentCharacter.OnOffControls(true);
                        break;
                    case (Tutorial.MirrorBroken):
                        if (_tutoMirrorBroken.activeSelf)
                        {
                            _tutoMirrorBroken.SetActive(false);
                            EventManager.instance.TutorialPauseGame(false);
                        }
                        //  GameManager.instance._currentCharacter.OnOffControls(true);
                        break;
                    case (Tutorial.MirrorInstructions):
                        if (_tutoMirror.activeSelf)
                        {
                            _tutoMirror.SetActive(false);
                            EventManager.instance.TutorialPauseGame(false);
                        }
                        //   GameManager.instance._currentCharacter.OnOffControls(true);
                        break;
                    case (Tutorial.Pilar):
                        //   GameManager.instance._currentCharacter.OnOffControls(true);
                        if (_tutoPilar.activeSelf)
                        {
                            _state = Tutorial.None;
                            TurnOffAll();
                            _tutoPilar.SetActive(false);
                            EventManager.instance.TutorialPauseGame(false);
                        }
                        else
                        {
                            _state = Tutorial.None;
                            _tutoPilar.SetActive(true);
                            EventManager.instance.TutorialPauseGame(true);
                        }

                        break;
                    case (Tutorial.DoubleJump):
                        if (_tutoDobleJump.activeSelf)
                        {
                            _tutoDobleJump.SetActive(false);
                            EventManager.instance.TutorialPauseGame(false);
                        }
                        //  GameManager.instance._currentCharacter.OnOffControls(true);
                        break;
                    case (Tutorial.Climb):
                        if (_tutoClimb.activeSelf)
                        {
                            _state = Tutorial.None;
                            _tutoClimb.SetActive(false);
                            _pressToAccesInfo.enabled = true;
                            _pressToAccesInfo2.SetActive(true);
                            EventManager.instance.TutorialPauseGame(false);
                        }
                        break;
                    case (Tutorial.FireThrower):
                        if (_trapInfoFireThrower.activeSelf)
                        {
                            _state = Tutorial.None;
                            TurnOffAll();
                            _trapInfoFireThrower.SetActive(false);
                            EventManager.instance.TutorialPauseGame(false);
                        }
                        else
                        {
                            _state = Tutorial.None;
                            _state = Tutorial.FireThrower;
                            _trapInfoFireThrower.SetActive(true);
                            EventManager.instance.TutorialPauseGame(true);
                        }
                        break;
                    case (Tutorial.Axe):
                        if (_trapInfoAxe.activeSelf)
                        {
                            _state = Tutorial.None;
                            TurnOffAll();
                            _trapInfoAxe.SetActive(false);
                            EventManager.instance.TutorialPauseGame(false);
                        }
                        else
                        {
                            _state = Tutorial.None;
                            _trapInfoAxe.SetActive(true);
                            EventManager.instance.TutorialPauseGame(true);
                        }
                        break;
                    case (Tutorial.Darts):
                        if(_trapInfoDarts.activeSelf)
                        {
                            _state = Tutorial.None;
                            TurnOffAll();
                            _trapInfoDarts.SetActive(false);
                            EventManager.instance.TutorialPauseGame(false);
                        }
                        else
                        {
                            _state = Tutorial.None;
                            _trapInfoDarts.SetActive(true);
                            EventManager.instance.TutorialPauseGame(true);
                        }
                        break;
                }
            }
        }
    }
    public void CollisionWithPlayer(Tutorial tutorialType)
    {
        //GameManager.instance._currentCharacter.OnOffControls(false);
        _state = tutorialType;
            EventManager.instance.TutorialPauseGame(true);
        switch (tutorialType)
        {
            case (Tutorial.Movement):
                break;
            case (Tutorial.Dash):
                TurnOffAll();
                _tutoDash.SetActive(true);
                break;
            case (Tutorial.Attack):
                StartCoroutine(AttackTutorial());
                _isWaiting = true;
                break;
            case (Tutorial.MirrorBroken):
                StartCoroutine(BrokenMirrorTutorial());
                _isWaiting = true;
                break;
            case (Tutorial.MirrorInstructions):
                StartCoroutine(MirrorInstructionsTutorial());
                _isWaiting = true;
                break;
            case (Tutorial.DoubleJump):
                TurnOffAll();
                _tutoDobleJump.SetActive(true);
                break;
            case (Tutorial.Climb):
                TurnOffAll();
                _tutoClimb.SetActive(true);
                break;
            case (Tutorial.JakeInfo):
                StartCoroutine(TutoJake());
                _isWaiting = true;
                break;
            case (Tutorial.TudorInfo):
                StartCoroutine(TutoTudor());
                _isWaiting = true;
                break;
            case (Tutorial.PressToAccesInfo):

                break;
        }
    }
    private bool _lifeState = true;
    public void EnableAndDisableLifebar(bool value)
    {
        if (value && _lifeState)
        {
            for (int i = 0; i < _lifeBar.Length; i++)
            {
                _lifeBarState[i] = _lifeBar[i].activeSelf;
                _lifeBar[i].SetActive(false);
            }
            _lifeState = false;
        }
        if(!value && !_lifeState)
        {
            for (int i = 0; i < _lifeBar.Length; i++)
            {
                _lifeBar[i].SetActive(_lifeBarState[i]);
            }
            _lifeState = true;
        }
    }
    private void PilarTutorial(bool value)
    {
            TurnOffAll();
            _tutoPilar.SetActive(value);
            _isWaiting = false;
    }
    private void DartThrowerTutorial(bool value)
    {
      
            TurnOffAll();
            _trapInfoDarts.SetActive(value);
            _isWaiting = false;
    }
    private void AxeTutorial(bool value)
    {
        TurnOffAll();
        _trapInfoAxe.SetActive(value);
        _isWaiting = !value;
        if (!value)
        {
            EventManager.instance.TutorialPauseGame(false);
            EnableAndDisableLifebar(false);
        }
    }
    private IEnumerator TutoTudor()
    {
        yield return new WaitForSeconds(0.7f);
        TurnOffAll();
        EventManager.instance.TutorialPauseGame(false);
        EventManager.instance.TutorialPauseGame(true);
        _tutoTudor.SetActive(true);
        _isWaiting = false;
    }
    public IEnumerator TutoJake()
    {
        yield return new WaitForSeconds(0.7f);
        TurnOffAll();
        EventManager.instance.TutorialPauseGame(false);
        EventManager.instance.TutorialPauseGame(true);
        _tutoJake.SetActive(true);
        _isWaiting = false;
    }
    private IEnumerator AttackTutorial()
    {
        yield return new WaitForSeconds(1.5f);
        TurnOffAll();
        _tutoAttack.SetActive(true);
        _isWaiting = false;
    }
    private IEnumerator ActualizeLifeBar()
    {
        yield return new WaitForSeconds(0.0001f);
        GameManager.instance.ActualizeLifeBar(true);
    }
    private IEnumerator BrokenMirrorTutorial()
    {
        yield return new WaitForSeconds(1f);
        TurnOffAll();
        _tutoMirrorBroken.SetActive(true);
        _mirrorPiece.SetActive(true);
        _isWaiting = false;
    }
    private IEnumerator MirrorInstructionsTutorial()
    {
        yield return new WaitForSeconds(1.5f);
        TurnOffAll();
        _tutoMirror.SetActive(true);
        _isWaiting = false;
    }
    public void TutoPlayer()
    {
        _tutoPlayer--;
        if (GameManager.instance._currentCharacter._character == BaseCharacter.Identity.jake)
        {
            ChangeLifeBar();
            CollisionWithPlayer(Tutorial.JakeInfo);
        }
        if (GameManager.instance._currentCharacter._character == BaseCharacter.Identity.Tudor)
        {
            ChangeLifeBar();
            CollisionWithPlayer(Tutorial.TudorInfo);
        }
        if (_tutoPlayer <= 0)
        {
            GameManager.instance.Changing -= TutoPlayer;
    }
        }
    public void Win()
    {
        EventManager.instance.TutorialPauseGame(true);
        _win.SetActive(true);
    }
    public void TurnOffAll()
    {
        if (_tutoBaseControls.activeSelf) 
        {
            _tutoBaseControls.SetActive(false);
        }
        if (_tutoDash.activeSelf)
        {
            _tutoDash.SetActive(false);
        }
        if (_tutoDobleJump.activeSelf)
        {
            _tutoDobleJump.SetActive(false);
        }
        if (_tutoClimb.activeSelf)
        {
            _tutoClimb.SetActive(false);
        }
        if (_tutoTudor.activeSelf)
        {
            _tutoTudor.SetActive(false);
        }
        if (_tutoJake.activeSelf)
        {
            _tutoJake.SetActive(false);
        }
        if (_tutoAttack.activeSelf)
        {
            _tutoAttack.SetActive(false);
        }
        if(_tutoMirrorBroken.activeSelf)
        {
            _tutoMirrorBroken.SetActive(false);
        }
        if (_tutoMirror.activeSelf)
        {
            _tutoMirror.SetActive(false);
        }
        if (_tutoPilar.activeSelf)
        {
            _tutoPilar.SetActive(false);
        }
        if (_trapInfoFireThrower.activeSelf)
        {
            _trapInfoFireThrower.SetActive(false);
        }
    }
    private Tutorial zone;
    private bool inZone;
    public void InZone(Tutorial type,bool value)
    {
        zone = type;
        inZone = value;
    }
    private bool _changed=true;
    private void ChangeLifeBar()
    {
        if (_changed)
        {
            _changed = false;
            _lifeBarState[2] = false;
            _lifeBarState[3] = true;
        }
        else
        {
            _changed = true;
            _lifeBarState[2] = true;
            _lifeBarState[3] = false;
        }
    }
}
