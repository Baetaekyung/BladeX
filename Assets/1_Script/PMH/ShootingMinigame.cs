using Swift_Blade.Combat.Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class ShootingMinigame : MonoBehaviour
    {
        [SerializeField] private GameObject rock5;
        [SerializeField] private GameObject boomBox;

        [SerializeField] private List<Transform> shootPosList;

        [SerializeField] private Transform PlayerTrm;

        [SerializeField] private GameObject nowObj;
        
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
                int listNansu = Random.Range(0, shootPosList.Count);
                float throwedNansu = Random.Range(0f, 100f);

                //만약 몇퍼센트 확률이라면 붐박스소환
                nowObj = Instantiate(throwedNansu > 30f ? rock5 : boomBox, shootPosList[listNansu].position, Quaternion.identity);

                bool compoExist = nowObj.TryGetComponent(out MinigameThrowed mt);
                bool compoExistTest = nowObj.TryGetComponent(out Bomb bb);
                if (compoExist || bb)
                {
                    Vector3 dir = (PlayerTrm.transform.position - shootPosList[listNansu].transform.position).normalized;
                    Debug.Log($"포스는 {dir} (포스기아님)");
                    if(mt)
                    {
                        mt.SetDirection(dir);
                    }
                    if(bb)
                    {
                        bb.SetDirection(dir);
                    }
                    Debug.Log("제가언제말이라도했습니까");
                }
                //throwRock.transform.GetComponent<BaseThrow>().SetDirection(Vector3.up);

                //throwRock.SetPhysicsState(false);

                //throwRock.GetComponent<BaseThrow>().SetDirection(Vector3.up);
                //Debug.Log(throwRock.GetComponent<BaseThrow>());
                yield return new WaitForSeconds(1);
            }
        }
    }
}
