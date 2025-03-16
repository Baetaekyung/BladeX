using UnityEngine;

namespace Swift_Blade
{
    public class LoadMinigames : MonoBehaviour
    {
        [SerializeField] private Transform PlayerTrm;
        [SerializeField] private Transform[] spawnPoints;
        private void Start()
        {
            int n = Random.Range(0, 2);
            PlayerTrm.position = spawnPoints[n].position;

            Debug.Log(spawnPoints[n].position);
        }
    }
}
