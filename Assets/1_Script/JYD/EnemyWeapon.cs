using UnityEngine;

namespace Swift_Blade.Enemy
{
    public class EnemyWeapon : MonoBehaviour
    {
        void Start()
        {
            transform.parent = null;
            gameObject.AddComponent<BoxCollider>();
            
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        }
    }
}