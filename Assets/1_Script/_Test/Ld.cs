using UnityEngine;
using UnityEngine.SceneManagement;

namespace Swift_Blade
{
    public class Ld : MonoBehaviour
    {
        public int index;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
                SceneManager.LoadScene(index);
        }
    }
}
