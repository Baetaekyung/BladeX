using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade
{
    public class TestScript2 : MonoBehaviour
    {
        [SerializeField] private TestMonoPoolTar targetPrefab;
        [SerializeField] private GameObject returnTarget;
        [SerializeField] private PoolPrefabGameObjectSO ppSO;
        private void Awake()
        {
            //GameObjectPoolManager.Initialize(ppSO);
            //GameObjectPoolManager.Initialize(ppSO);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                //MonoGenericPool<TestMonoPoolTar>.Pop();
                GameObjectPoolManager.Pop(ppSO);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                //MonoGenericPool<TestMonoPoolTar>.Push(returnTarget);
                GameObjectPoolManager.Push(ppSO, returnTarget);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                //print(MonoGenericPool<TestMonoPoolTar>.Dbg_print());
                print(GameObjectPoolManager.Dbg(ppSO));
            }

        }
    }
}
