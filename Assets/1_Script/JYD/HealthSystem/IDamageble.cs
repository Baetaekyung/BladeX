using System;
using UnityEngine;

public interface IDamageble
{
    public void TakeDamage(ActionData actionData);
    public void TakeHeal();
    public void Dead();
            
}
