using System;
using System.Collections;
using UnityEngine;

namespace Swift_Blade
{
    public class ParryLight : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(nameof(ActiveFalseSelf));
        }

        private IEnumerator ActiveFalseSelf()
        {
            yield return new WaitForSeconds(0.15f);
            gameObject.SetActive(false);
        }
    }
}
