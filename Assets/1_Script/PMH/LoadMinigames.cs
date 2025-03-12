using UnityEngine;

namespace Swift_Blade
{
    public class LoadMinigames : MonoBehaviour
    {
        [SerializeField] private Transform PlayerTrm;
        [SerializeField] private Transform[] spawnPoints;
        private void Start()
        {
            int n = Random.Range(0, spawnPoints.Length);
            PlayerTrm.position = spawnPoints[n].position;

            Debug.Log("講馬馬馬馬馬馬馬");
        }
    }
}
