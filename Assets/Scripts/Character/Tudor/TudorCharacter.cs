using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TudorCharacter : BaseCharacter
{
    [SerializeField] private TudorModel _tudor;
    private TudorView _tudorView;
    private TudorController _tudorController;
    private PlayerAttack _playerAttack;
    private Action _updateDelegate;
    private JumpingFoot _playerJump;

    protected override void Start()
    {
        base.Start();
        _tudor = (TudorModel)_model;
        _tudorView = gameObject.AddComponent<TudorView>().SetModel(_tudor);
        _controller = new TudorController(_tudor);
        _tudorController = (TudorController)_controller;
        _playerJump = _tudor.GetJumpingFoot();
        _playerJump.isGrounded += _tudorView.JumpingState;
        _playerJump.hasJumped += _tudorView.Jumped;
        _tudor.puttingMirror += _tudorView.PutTheMirror;
        _tudor.changeCharacter += _tudorView.JumpInTheMirror;
        _tudor.changeCharacterOut += _tudorView.JumpFromTheMirror;
        _tudor.changeCharacter += _tudorController.ChangeCharacter;
        _tudor.climbing += _tudorView.Climb;
        _tudor.climbtop += _tudorView.ReachTheTop;

        _playerAttack = _tudor.GetPlayerAttak();
        _playerAttack.isAttacking += _tudorView.Attack_01;
        _updateDelegate += _tudorController.OnUpdate;
        _updateDelegate += _tudorView.OnUpdate;
    }
    private void Update()
    {
        _updateDelegate?.Invoke();
    }
    
    public override void Initialize(Transform t)
    {
        transform.position = t.position;
        transform.rotation = t.rotation;
        _tudor.GetOutOfTheMirror();
    }
    public override void ChangeCharacter()
    {
        GameManager.instance.ChangeCharacters(_characterToChange, transform);
        gameObject.SetActive(false);
    }
    public override void OnOffControls(bool value)
    {
        _tudorController.TurnOnOffPlayerControlls(value);
    }
}
