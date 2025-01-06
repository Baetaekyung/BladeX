using UnityEngine;

namespace Swift_Blade
{
    public class StaticCamera : MonoBehaviour
    {
        [SerializeField] private Transform followCamera;
        private void Update()
        {
            transform.SetPositionAndRotation(followCamera.position, followCamera.rotation);
        }
    }
}
