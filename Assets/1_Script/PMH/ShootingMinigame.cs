using System.Collections;
using UnityEngine;

namespace Swift_Blade
{
    public class ShootingMinigame : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(nameof(ShootingCoroutine));
        }

        private IEnumerator ShootingCoroutine()
        {
            
            Debug.Log("นึ");
            yield return new WaitForSeconds(1);
        }
    }
}
