
using System;
using DG.Tweening;
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
                
            sword.AddComponent<EnemyWeapon>();
        }
        
    }
}
