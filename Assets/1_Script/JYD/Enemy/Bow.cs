using System;
using UnityEngine;

namespace Swift_Blade.Enemy.Bow
{
    public class Bow : EnemyWeapon
    {
        protected override void Awake()
        {
            base.Awake();
            GetComponent<LineRenderer>().enabled = false;
            rigidbody.mass = 120f;
        }
        
    }
}
