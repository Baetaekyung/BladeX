using Swift_Blade.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon/GreatSword")]
    public class GreatSwordWeaponSO : WeaponSO
    {
        [SerializeField] private PoolPrefabMonoBehaviourSO hexagonParticle;
        protected override void PlayParticle()
        {
            MonoGenericPool<HexagonParticle>.Initialize(this.hexagonParticle);
            
            HexagonParticle hexagonParticle = MonoGenericPool<HexagonParticle>.Pop();
            hexagonParticle.transform.SetParent(playerTransform);
            hexagonParticle.transform.position = playerTransform.position + new Vector3(0,1f,0);
        }
    }
}