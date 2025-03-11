using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class BallGenerator : MonoBehaviour
    {
        [SerializeField] private float tickRate = 1;

        [SerializeField] private Transform spawnPointParentTrm;
        [SerializeField] private List<Transform> spawnPoints;

        [SerializeField] private GameObject RollingRock;
        [SerializeField] private GameObject BrokingItem;
        [SerializeField] private GameObject ToCoinItem;


        [SerializeField] private Vector4[] patthon;
        //0����
        //1��
        //2������
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
                case 0: //�ƹ��͵�����.
                    //
                    break;
                case 1: //��
                    Instantiate(RollingRock, spawnPoints[n].position, Quaternion.identity);
                    break;
                case 2: //������
                    //Instantiate(RollingRock, spawnPoints[2].position, Quaternion.identity);
                    break;
                case 3: //����
                    //Instantiate(RollingRock, spawnPoints[3].position, Quaternion.identity);
                    break;
            }
        }
    }
}
