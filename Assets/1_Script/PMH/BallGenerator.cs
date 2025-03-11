using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class BallGenerator : MonoSingleton<BallGenerator>
    {
        [SerializeField] private float tickRate = 1;

        [SerializeField] private Transform spawnPointParentTrm;
        [SerializeField] private List<Transform> spawnPoints;

        [SerializeField] private GameObject RollingRock;
        [SerializeField] private GameObject BrokingItem;
        [SerializeField] private GameObject ToCoinItem;
        [SerializeField] private GameObject JustCoin;

        [SerializeField] private List<Transform> rockAndItemList;

        [SerializeField] private Vector4[] patthon;
        //0없음
        //1돌
        //2아이템
        private void Start()
        {
            foreach(Transform t in spawnPointParentTrm.transform)
            {
                spawnPoints.Add(t);
            }
            StartCoroutine("BallGenerateCoroutine");
        }

        private IEnumerator BallGenerateCoroutine()
        {
            while(true)
            {
                SpawnBall();
                yield return new WaitForSeconds(tickRate);
            }
        }

        private void SpawnBall()
        {
            int n = Random.Range(0, spawnPoints.Count);

            Debug.Log(Mathf.FloorToInt(patthon[n].x));
            InstantiateRock(Mathf.FloorToInt(patthon[n].x), 0);
            Debug.Log(Mathf.FloorToInt(patthon[n].y));
            InstantiateRock(Mathf.FloorToInt(patthon[n].y), 1);
            Debug.Log(Mathf.FloorToInt(patthon[n].z));
            InstantiateRock(Mathf.FloorToInt(patthon[n].z), 2);
            Debug.Log(Mathf.FloorToInt(patthon[n].w));  
            InstantiateRock(Mathf.FloorToInt(patthon[n].w), 3);
        }

        private void InstantiateRock(int pn, int n)
        {
            switch (pn)
            {
                case 0: //아무것도없음.
                    //
                    break;
                case 1: //돌
                    GameObject item1 = Instantiate(RollingRock, spawnPoints[n].position, Quaternion.identity);
                    rockAndItemList.Add(item1.transform);
                    break;
                case 2: //아이템
                    int rn = Random.Range(0, 2);
                    if(rn == 0)
                    {
                        GameObject item2 = Instantiate(ToCoinItem, spawnPoints[n].position, Quaternion.identity);
                        rockAndItemList.Add(item2.transform);
                    }
                    else
                    {
                        GameObject item2 = Instantiate(BrokingItem, spawnPoints[n].position, Quaternion.identity);
                        rockAndItemList.Add(item2.transform);
                    }
                        
                    break;
                case 3: //코인
                    GameObject item3 = Instantiate(JustCoin, spawnPoints[n].position, Quaternion.identity);
                    rockAndItemList.Add(item3.transform);
                    break;
            }
        }

        public void RemoveMeInList(Transform target)
        {
            rockAndItemList.Remove(target);
        }

        public void AllObjectToCoin()
        {
            foreach(Transform item in rockAndItemList)
            {
                Instantiate(JustCoin, item.position, Quaternion.identity);
                Destroy(item.gameObject);
            }
        }
    }
}
