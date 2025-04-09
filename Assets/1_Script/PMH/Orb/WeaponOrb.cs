using DG.Tweening;
using System;
using UnityEngine;

namespace Swift_Blade
{
    public class WeaponOrb : BaseOrb
    {
        [SerializeField] private WeaponSO weapon;

        protected override TweenCallback CreateDefaultCallback()
        {
            return () => { Debug.Log("onend" + weapon.name); Destroy(gameObject); };
        }

    }
}
