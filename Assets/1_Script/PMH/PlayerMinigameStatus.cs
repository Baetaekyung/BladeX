using System.Collections;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerMinigameStatus : MonoSingleton<PlayerMinigameStatus>
    {
        [field : SerializeField] public bool isCanBrokingrock;

        public void GetCanBrokingItem()
        {
            isCanBrokingrock = false;
            StopAllCoroutines();
            StartCoroutine("GetItemEvents");
        }
        private IEnumerator GetItemEvents()
        {
            isCanBrokingrock = true;
            Debug.Log("무적이되.");
            yield return new WaitForSeconds(5);
            isCanBrokingrock = false;
            Debug.Log("다적이되.");
        }
    }
}
