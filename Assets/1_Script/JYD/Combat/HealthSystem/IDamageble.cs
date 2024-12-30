using System;
using UnityEngine;

public interface IDamageble
{
    public void TakeDamage(ActionData actionData);
    public void TakeHeal(float amount);
    public void Dead();
            
}
