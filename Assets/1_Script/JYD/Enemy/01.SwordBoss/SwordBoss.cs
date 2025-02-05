
using System;
using Swift_Blade.Enemy;
using UnityEngine;
using UnityEngineInternal;

namespace Swift_Blade.Enemy.Boss.Sword
{
    public class SwordBoss : BaseEnemy
    {
        public GameObject sword;

        public override void SetDead()
        {
            base.SetDead();

            sword.transform.parent = null;
            sword.AddComponent<BoxCollider>();
            sword.AddComponent<Rigidbody>();
        }
    }
}
