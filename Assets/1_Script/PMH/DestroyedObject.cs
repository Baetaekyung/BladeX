using UnityEngine;

namespace Swift_Blade
{
    public class DestroyedObject : MonoBehaviour
    {
        [SerializeField] private float laterDestTime;
        void OnEnable()
        {
            Destroy(gameObject, laterDestTime);
        }
    }
}
