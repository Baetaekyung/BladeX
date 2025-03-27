using UnityEngine;
using System;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "SceneManager", menuName = "SO/Scene/SceneManager")]
    public class SceneManagerSO : ScriptableObject
    {
        public event Action LevelClearEvent;
        public event Action<string,Action> SceneLoadEvent;
        public event Action SceneEnterEvent;
        
        public void LevelClear()
        {
            LevelClearEvent?.Invoke();
        }
        
        public void LoadScene(string sceneName)
        {
            SceneLoadEvent?.Invoke(sceneName,SceneEnterEvent);
        }
        
    }
}
