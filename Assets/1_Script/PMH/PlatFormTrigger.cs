using System.Collections;
using UnityEngine;

namespace Swift_Blade
{
    public class PlatFormTrigger : MonoBehaviour
    {
        [SerializeField] private Vector3 CurrentPosition;
        [SerializeField] private Vector3 DestinationPosition;
        private void Start()
        {
            CurrentPosition = transform.position;

            StartCoroutine(MovePlatformCoroutine());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("범위 안에 들아오다");
                
            }
        }

        IEnumerator MovePlatformCoroutine()
        {
            while (true)
            {
                
                yield return new WaitForSecondsRealtime(1);
            }
        }
    }
}
