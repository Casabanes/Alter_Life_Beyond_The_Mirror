using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLifeComponent : LifeComponent
{
    protected override void Start()
    {
        _actualLife = _maxLife;
    }
    public override void SuscribeToWinLoseManager()
    {
    }
    public override void UnSuscribeToWinLoseManager()
    {
    }
}
