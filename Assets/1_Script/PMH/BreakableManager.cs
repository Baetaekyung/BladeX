using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class BreakableManager : MonoBehaviour
    {
        [SerializeField] private List<BreakableObject> breakableObjects;

        [SerializeField] private CoinManager coinManager;
        void Start()
        {
            foreach(BreakableObject obj in breakableObjects)
            {
                obj.OnGameObjectDestroy += Breakable;
            }
        }

        private void Breakable(BreakableObject obj)
        {
            Debug.Log("�μ����� �� ��Żó��");
            RandomGetMoney();

            obj.OnGameObjectDestroy -= Breakable;
            breakableObjects.Remove(obj);
        }

        private void RandomGetMoney()
        {
            float randomNumber = Random.Range(0.0f, 100.0f);
            if(randomNumber >= 50.0f)
            {
                print("�ϼ����� ����");
                coinManager.AddedCountCoin(100);
            }
        }
    }
}
