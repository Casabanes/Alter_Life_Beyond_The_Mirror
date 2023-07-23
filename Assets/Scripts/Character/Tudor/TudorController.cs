using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TudorController : BaseController
{
    public enum PlayerActions
    {
        idle,
        moving,
        attacking,
        puttingMirror,
        changeCharacter,
        climb,
        WithOutControls
    };
        private BaseModel _player;
        private TudorModel _tudor;
        private PlayerMovement _movement;
        private PlayerAttack _playerAttack;
        private RotationofSpeed _rotationofSpeed;
        private float x;
        private float z;
        private const int _constZero = 0;
        private Action _updateDelegate;
        private PlayerActions _currentAction;
        private bool isDead;
        public TudorController(BaseModel player)
        {
            _player = player;
            _tudor = (TudorModel)player;
            _movement = _tudor.GetMovement();
            _playerAttack = _tudor.GetPlayerAttak();
            _rotationofSpeed = _tudor.GetRotationOfSpeed();
            AssignCurrentActionDelegates();
            _currentAction = PlayerActions.idle;
            ActionsCheck();
            EventManager.instance.pause += TurnOnOffPlayerControlls;
    }
    public override void OnUpdate()
        {
            CheckAxis();
            ActionsCheck();
            _updateDelegate();
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
            _tudor.PutTheMirror();
            }
        }
    public void Climb()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _tudor.Climb();
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
                    _updateDelegate += Attack;
                    _updateDelegate += PutTheMirror;
                    _updateDelegate += Climb;
                    break;
                case PlayerActions.moving:
                    _updateDelegate = delegate { };
                    _updateDelegate = Movement;
                    _updateDelegate += Attack;
                    _updateDelegate += Rotating;
                    _updateDelegate += PutTheMirror;
                    _updateDelegate += Climb;
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
        }
        #region CurrentAction
        public void AssignCurrentActionDelegates()
        {
            _movement.isMoving += MoveOrNotMove;
            _playerAttack.isAttacking += AttackOrNotAttack;
            _tudor.puttingMirror += PuttingTheMirror;
            _tudor.climbing+=ClimbAction;
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
    public void ClimbAction(bool value)
    {
        if (value)
        {
            _currentAction = PlayerActions.climb;
        }
        else
        {
            _currentAction = PlayerActions.idle;
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
