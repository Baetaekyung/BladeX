using UnityEngine;

namespace Swift_Blade.Enemy.Boss.Sword
{
    public class Sword : MonoBehaviour
    {
        void Start()
        {
            gameObject.AddComponent<BoxCollider>();
            
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true; 
                        
        }
    }
}