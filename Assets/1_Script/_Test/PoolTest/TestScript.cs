using UnityEngine;
using Swift_Blade.Pool;

namespace Swift_Blade
{
    public class TestScript : MonoBehaviour
    {
        [SerializeField] private TestMonoPoolTar targetPrefab;
        [SerializeField] private TestMonoPoolTar returnTarget;
        [SerializeField] private PoolPrefabMonoBehaviourSO ppSO;
        private void Awake()
        {
            //GameObjectPoolManager.Initialize(ppSO);
            MonoGenericPool<TestMonoPoolTar>.Initialize(ppSO);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                MonoGenericPool<TestMonoPoolTar>.Pop();
                //GameObjectPoolManager.Pop(ppSO);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                MonoGenericPool<TestMonoPoolTar>.Push(returnTarget);
                //GameObjectPoolManager.Push(ppSO, returnTarget);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                //print(MonoGenericPool<TestMonoPoolTar>.Dbg_print());
                //print(GameObjectPoolManager.Dbg(ppSO));
            }

        }
    }
}
