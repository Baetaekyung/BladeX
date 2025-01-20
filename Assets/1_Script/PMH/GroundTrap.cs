using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Swift_Blade
{
    public class GroundTrap : MonoBehaviour
    {
        [SerializeField] private bool isActive = false;

        [SerializeField] private float delay;
        [SerializeField] private Transform trapSpears;

        private void Start()
        {
            trapSpears.localPosition = new Vector3(0, (isActive ? 0 : -2), 0);

            StartCoroutine(ActiveCoroutine());
        }

        IEnumerator ActiveCoroutine()
        {
            while (true)
            {
                if (isActive)
                {
                    trapSpears.DOMoveY(transform.localPosition.y - 2, 0.5f);
                    isActive = false;
                }
                else
                {
                    trapSpears.DOMoveY(transform.localPosition.y, 0.5f);
                    isActive = true;
                }

                yield return new WaitForSeconds(delay);
            }
        }
    }
}
