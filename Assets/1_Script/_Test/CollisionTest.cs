using UnityEngine;

namespace Swift_Blade
{
    public class CollisionTest : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            print("oce");
        }
        private void OnCollisionStay(Collision collision)
        {
            print("ocs");
        }
        private void OnCollisionExit(Collision collision)
        {
            print("ocexit");
        }
    }
}
