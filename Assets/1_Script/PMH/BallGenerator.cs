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

        private int patthonCount;
        //0없음
        //1돌
        //2아이템
        private void Start()
        {
            foreach(Transform t in spawnPointParentTrm.transform)
            {
                spawnPoints.Add(t);
            }
            patthonCount = 0;
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
            if(patthonCount >= patthon.Length)
            {
                patthonCount = 0;
            }

            //Debug.Log(Mathf.FloorToInt(patthon[n].x));
            InstantiateRock(Mathf.FloorToInt(patthon[patthonCount].x), 0);
            //Debug.Log(Mathf.FloorToInt(patthon[n].y));
            InstantiateRock(Mathf.FloorToInt(patthon[patthonCount].y), 1);
            //Debug.Log(Mathf.FloorToInt(patthon[n].z));
            InstantiateRock(Mathf.FloorToInt(patthon[patthonCount].z), 2);
            //.Log(Mathf.FloorToInt(patthon[n].w));  
            InstantiateRock(Mathf.FloorToInt(patthon[patthonCount].w), 3);
            Debug.Log(patthonCount + " 번째 소환!!!!!!!!!!!!!!");
            patthonCount++;
        }

        private void InstantiateRock(int pn, int spawnPoints)
        {
            switch (pn)
            {
                case 0: //아무것도없음.
                    //
                    break;
                case 1: //돌
                    GameObject item1 = Instantiate(RollingRock, this.spawnPoints[spawnPoints].position, Quaternion.identity);
                    rockAndItemList.Add(item1.transform);
                    break;
                case 2: //아이템
                    int rn = Random.Range(0, 2);
                    if(rn == 0)
                    {
                        GameObject item2 = Instantiate(ToCoinItem, this.spawnPoints[spawnPoints].position, Quaternion.identity);
                        rockAndItemList.Add(item2.transform);
                    }
                    else
                    {
                        GameObject item2 = Instantiate(BrokingItem, this.spawnPoints[spawnPoints].position, Quaternion.identity);
                        rockAndItemList.Add(item2.transform);
                    }
                        
                    break;
                case 3: //코인
                    GameObject item3 = Instantiate(JustCoin, this.spawnPoints[spawnPoints].position, Quaternion.identity);
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
                if(item.TryGetComponent<RockToCoin>(out RockToCoin rtc))
                {
                    Destroy(item.gameObject);
                }
                else
                {
                    Instantiate(JustCoin, item.position, Quaternion.identity);
                    Destroy(item.gameObject);
                }
            }
        }
    }
}
