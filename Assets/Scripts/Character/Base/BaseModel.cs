using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseModel : MonoBehaviour
{

    public int _maxMirrorPieces;
    public int _currentMirrorPieces;
    public CapsuleCollider _capsuleCollider;
    public LayerMask layerToCheckCanMove;


    public virtual void TakeMirrorFragments()
    {
        if (_currentMirrorPieces == _maxMirrorPieces)
        {
            return;
        }
        _currentMirrorPieces++;
    }
    public virtual float GetMovementYvalue()
    {
        return 0;
    }
}
