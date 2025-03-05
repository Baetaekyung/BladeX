using Swift_Blade.Combat.Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class ShootingMinigame : MonoBehaviour
    {
        [SerializeField] private BaseThrow rock5;
        [SerializeField] private List<Transform> shootPosList;

        [SerializeField] private Transform PlayerTrm;
        void Start()
        {
            foreach(Transform t in transform)
            {
                shootPosList.Add(t);
            }
            StartCoroutine("Ming");
        }

        private IEnumerator Ming()
        {
            while (true)
            {
                //Vector3 pos = transform.Find("Player").position;
                int n = Random.Range(0, shootPosList.Count);
                GameObject throwRock = Instantiate(rock5.gameObject, shootPosList[n].position, Quaternion.identity);

                //throwRock.SetPhysicsState(false);
                //throwRock.GetComponent<BaseThrow>().SetDirection(Vector3.up);
                Debug.Log("นึ");
                yield return new WaitForSeconds(1);
            }
        }
    }
}
