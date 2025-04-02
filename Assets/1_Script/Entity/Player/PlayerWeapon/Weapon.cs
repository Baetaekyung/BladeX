using UnityEngine;

namespace Swift_Blade
{

    [CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon")]
    public class Weapon : ScriptableObject
    {
        [field: SerializeField] public GameObject LeftHandUsage { get; private set; }
        [field: SerializeField] public GameObject RightHandUsage { get; private set; }

        [field: SerializeField] public RuntimeAnimatorController WeaponAnimator { get; private set; }
    }
}
