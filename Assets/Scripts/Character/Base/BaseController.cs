using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController 
{
    public virtual void OnUpdate() { }
    public virtual void DeathOrRevive(bool value){}
    public virtual void ChangeCharacter(bool value) { }

}
