using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlayerActions 
{
    idle,
    moving,
    jumping,
    attacking,
    puttingMirror,
    changeCharacter,
    WithOutControls
};
public class PlayerController : BaseController
{
    private BaseModel _player;
    private PlayerModel _jake;
    private PlayerMovement _movement;
    private JumpingFoot _jumpingFoot;
    private PlayerAttack _playerAttack;
    private RotationofSpeed _rotationofSpeed;
    private float x;
    private float z;
    private const int _constZero = 0;
    private Action _updateDelegate;
    private PlayerActions _currentAction;
    private bool isDead;
    private Action _checkingActions;
    private bool _hasWeapon;
    private bool _hasMirror;
    public PlayerController(BaseModel player)
    {
        _player = player;
        _jake = (PlayerModel)player;
        _movement = _jake.GetMovement();
        _playerAttack = _jake.GetPlayerAttak();
        AsignJumpingFoot();
        _rotationofSpeed = _jake.GetRotationOfSpeed();
        AssignCurrentActionDelegates();
        _currentAction = PlayerActions.idle;
        ActionsCheck();
        _checkingActions += ActivateAttack;
        _checkingActions += ActivatePuttingMirror;
        EventManager.instance.pause += TurnOnOffPlayerControlls;
    }
    public override void OnUpdate()
    {
        CheckAxis();
        ActionsCheck();
        _updateDelegate();
    }
    public void AsignJumpingFoot()
    {
        if (_jumpingFoot == null)
        {
            _jumpingFoot = _jake.GetJumpingFoot();
        }
    }
    public void CheckAxis()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
    }
    public void Movement()
    {
        if (x != _constZero || z != _constZero)
        {
            _movement.Move(x, z);
        }
        else
        {
            _movement.DontMoving();
        }
    }
    public void AirMovment()
    {
        if (x != _constZero || z != _constZero)
        {
            _movement.AirMovement(x, z);
        }
        else
        {
            _movement.DontMoving();
        }
    }
    public void Dash()
    {
        if (_currentAction == PlayerActions.jumping)
        {
            _movement.StopRunning();
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
        {
            _movement.StartRunning();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _movement.StopRunning();
        }
    }
    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jumpingFoot.Jump();
        }
    }
    public void Rotating()
    {
        _movement.MoveDirectionInvoke(x, z);

    }
    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _playerAttack.Attack();
        }
    }
    public void PutTheMirror()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _jake.PutTheMirror();
        }
    }
    public override void ChangeCharacter(bool value)
    {
        if (value)
        {
            _rotationofSpeed.enabled = false;
            _currentAction = PlayerActions.changeCharacter;
        }
        else
        {
            _rotationofSpeed.enabled = true;
            _currentAction = PlayerActions.idle;
        }
    }
    private void ActionsCheck()
    {
        if (isDead)
        {
            _updateDelegate = delegate { };
            return;
        }
        switch (_currentAction)
        {
            case (PlayerActions.idle):
                _updateDelegate = delegate { };
                _updateDelegate = Movement;
                _updateDelegate += Jump;
                _updateDelegate += Attack;
                _updateDelegate += PutTheMirror;
                break;
            case PlayerActions.moving:
                _updateDelegate = delegate { };
                _updateDelegate = Movement;
                _updateDelegate += Dash;
                _updateDelegate += Jump;
                _updateDelegate += Attack;
                _updateDelegate += Rotating;
                _updateDelegate += PutTheMirror;
                break;
            case PlayerActions.jumping:
                _updateDelegate = delegate { };
                _updateDelegate = AirMovment;
                _updateDelegate += Rotating;
                _updateDelegate += Jump;
                break;
            case PlayerActions.attacking:
                _updateDelegate = delegate { };
                _updateDelegate += Rotating;
                break;
            case PlayerActions.puttingMirror:
                _updateDelegate = delegate { };
                break;
            case PlayerActions.changeCharacter:
                _updateDelegate = delegate { };
                break;
            case PlayerActions.WithOutControls:
                _updateDelegate = delegate { };
                break;
        }
        _checkingActions?.Invoke();
    }
    #region CurrentAction
    public void AssignCurrentActionDelegates()
    {
        _movement.isMoving += MoveOrNotMove;
        _jumpingFoot.isGrounded += JumpOrNotJump;
        _playerAttack.isAttacking += AttackOrNotAttack;
        _jake.puttingMirror += PuttingTheMirror;
    }
    public void MoveOrNotMove(bool value)
    {
        if (_currentAction == PlayerActions.idle)
        {
            if (value)
            {
                _currentAction = PlayerActions.moving;
            }
        }
        if (_currentAction == PlayerActions.moving)
        {
            if (!value)
            {
                _currentAction = PlayerActions.idle;
            }
        }
    }
    public void JumpOrNotJump(bool value)
    {
        if (_currentAction == PlayerActions.idle || _currentAction == PlayerActions.moving)
        {
            if (!value)
            {
                _movement.StopRunning();
                _currentAction = PlayerActions.jumping;
            }
        }
        if (_currentAction == PlayerActions.jumping)
        {
            if (value)
            {
                _currentAction = PlayerActions.idle;
            }
        }
    }
    public void AttackOrNotAttack(bool value)
    {
        if (_currentAction == PlayerActions.idle || _currentAction == PlayerActions.moving)
        {
            if (value)
            {
                _movement.DontMoving();
                _rotationofSpeed.TimeToInterpolateIsLowOrNot(true);
                _currentAction = PlayerActions.attacking;
            }
        }
        if (!value)
        {
            _currentAction = PlayerActions.idle;
            _rotationofSpeed.TimeToInterpolateIsLowOrNot(false);
        }
    }
    public void PuttingTheMirror(bool value)
    {
        if (value)
        {
            _currentAction = PlayerActions.puttingMirror;
        }
        else
        {
            _currentAction = PlayerActions.idle;
        }
    }
    public void TurnOnOffPlayerControlls(bool value)
    {
        if (value)
        {
            _movement.DontMoving();
            _currentAction = PlayerActions.WithOutControls;
        }
        else
        {
            _currentAction = PlayerActions.idle;
        }
    }
    private void ActivateAttack()
    {
        if (_hasWeapon)
        {
            _checkingActions -= ActivateAttack;
        }
        else
        {
            _updateDelegate -= Attack;
        }
    }
    public void GetFacon()
    {
        _hasWeapon = true;
        ActionsCheck();
    }
    public void GetMirror()
    {
        _hasMirror = true;
        ActionsCheck();
    }
    private void ActivatePuttingMirror()
    {
        if(_hasMirror)
        {
            _checkingActions -= ActivatePuttingMirror;
        }
        else
        {
            _updateDelegate -= PutTheMirror;
        }
    }
    public override void DeathOrRevive(bool value)
    {
        base.DeathOrRevive(value);
        isDead = value;
        _movement.Move(0, 0);
    }
    #endregion
}
