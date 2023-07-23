using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerCharacter : BaseCharacter
{
    private PlayerView _playerView;
    private PlayerModel _jake;
    private JumpingFoot _playerJump;
    private PlayerAttack _playerAttack;
    private PlayerController _playerController;
    private Action _updateDelegate;
    protected override void Start()
    {
        base.Start();
        _jake = (PlayerModel)_model;
        _playerView = new PlayerView().SetModel(_jake);
        _controller = new PlayerController(_jake);
        _playerController = (PlayerController)_controller;
        _playerJump = _jake.GetJumpingFoot();
        _playerJump.isGrounded += _playerView.JumpingState;
        _playerJump.hasJumped += _playerView.Jumped;
        _jake.puttingMirror+= _playerView.PutTheMirror;
        _jake.changeCharacter += _playerView.JumpInTheMirror;
        _jake.changeCharacterOut += _playerView.JumpFromTheMirror;
        _jake.changeCharacter += _controller.ChangeCharacter;
        _jake.GetFacon += _playerController.GetFacon;
        _playerAttack = _jake.GetPlayerAttak();
        _playerAttack.isAttacking += _playerView.Attack_01;
        _jake.GetFacon += _playerView.PickUp;
        _updateDelegate += _controller.OnUpdate;
        _updateDelegate += _playerView.OnUpdate;
    }
    private void Update()
    {
        _updateDelegate?.Invoke();
    }
    public override BaseController GetController()
    {
        return _controller;
    }
    public PlayerController GetPlayerController()
    {
        return (PlayerController)_controller;
    }
    public override void Initialize(Transform t) 
    {
        transform.position = t.position;
        transform.rotation = t.rotation;
        _jake.GetOutOfTheMirror();
    }
    public override void ChangeCharacter() 
    {
        GameManager.instance.ChangeCharacters(_characterToChange,transform);
        gameObject.SetActive(false);
    }
    public override void OnOffControls(bool value)
    {
        _playerController.TurnOnOffPlayerControlls(value);
    }
}
