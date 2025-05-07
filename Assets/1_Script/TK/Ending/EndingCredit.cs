using System.Collections;
using UnityEngine;

namespace Swift_Blade
{
    public class EndingCredit : MonoBehaviour
    {
        [SerializeField] private float creditGenerateInterval;
        [TextArea]
        [SerializeField] private string[] creditMessages;
        [SerializeField] private EndingMessage endingPrefab;

        private WaitForSeconds _cacheWfs;

        private void Start()
        {
            _cacheWfs = new WaitForSeconds(creditGenerateInterval);

            StartCoroutine(nameof(EndingRoutine));
        }

        private IEnumerator EndingRoutine()
        {
            int currentIndex = 0;

            while(currentIndex < creditMessages.Length)
            {
                EndingMessage message = Instantiate(endingPrefab, transform);
                message.SetText(creditMessages[currentIndex]);

                yield return _cacheWfs;

                currentIndex++;
            }
        }
    }
}
