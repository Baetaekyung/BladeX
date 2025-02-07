using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade.Pool
{
    public static class GameObjectPoolManager
    {
        private readonly static Dictionary<int, GameObjectPool> gameObjectPoolDictionary;
        private const int DictionaryCapacity = 10;//todo : get prefab count from so
        static GameObjectPoolManager()
        {
            gameObjectPoolDictionary = new Dictionary<int, GameObjectPool>(DictionaryCapacity);
        }
        public static int Dbg(PoolPrefabGameObjectSO poolPrefabSO)
        {
            return gameObjectPoolDictionary[poolPrefabSO.GetHash].Dbg_Cnt();
        }
        private static GameObjectPool CreateDictionary(PoolPrefabGameObjectSO prefabSO)
        {
            GameObjectPool result = new GameObjectPool(prefabSO.GetPrefab, preCreate: prefabSO.GetPreCreate);
            gameObjectPoolDictionary.Add(prefabSO.GetHash, result);
            return result;
        }
        public static void Initialize(PoolPrefabGameObjectSO prefabSO)
        {
            int hash = prefabSO.GetHash;
            GameObject prefab = prefabSO.GetPrefab;

            bool collisionCheck = !gameObjectPoolDictionary.ContainsKey(prefabSO.GetHash);
            Debug.Assert(collisionCheck, $"Trying to add a key that has been added to the dictionary. {prefab.name}{hash}");

            CreateDictionary(prefabSO);
        }
        public static void Initialize(IEnumerable<PoolPrefabGameObjectSO> poolPrefabSOs)
        {
            foreach (var item in poolPrefabSOs)
            {
                Initialize(item);
            }
        }
        public static GameObject Pop(PoolPrefabGameObjectSO prefabSO)
        {
            GameObject result;
            if (gameObjectPoolDictionary.TryGetValue(prefabSO.GetHash, out GameObjectPool value))
                result = value.Pop();
            else
            {
                Debug.Assert(true, "runtimeInitializing! call func:Initialize before calling this");
                GameObjectPool gameObjectPool = CreateDictionary(prefabSO);
                result = gameObjectPool.Pop();
            }
            return result;
        }
        public static void Push(PoolPrefabGameObjectSO prefabSO, GameObject instance)
        {
            Debug.Assert(instance != null, "local: push instance is null");

            if (gameObjectPoolDictionary.TryGetValue(prefabSO.GetHash, out GameObjectPool value))
                value.Push(instance);
            else
            {
                Debug.Assert(true, "runtimeInitializing! call func:Initialize before calling this");
                GameObjectPool gameObjectPool = CreateDictionary(prefabSO);
                gameObjectPool.Push(instance);
            }
        }
        public static void Clear(PoolPrefabGameObjectSO prefabSO)
        {
            gameObjectPoolDictionary[prefabSO.GetHash].Clear();
        }
        public static void ClearAll()
        {
            foreach (GameObjectPool item in gameObjectPoolDictionary.Values)
            {
                item.Clear();
            }
        }
    }
}
