using UnityEngine;

namespace Swift_Blade
{
    public class BGMChanger : MonoBehaviour
    {
        private void Start()
        {
            if (BGM.HasInit)
            {
                BGM.Instance.Stop();
                BGM.Instance.Kill();
            }
        }
    }
}
