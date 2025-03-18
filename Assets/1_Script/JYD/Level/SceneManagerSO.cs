using System;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "SceneManager", menuName = "SO/Scene/SceneManager")]
    public class SceneManagerSO : ScriptableObject
    {
        public event Action LevelClearEvent;
        public event Action<string,Action> SceneLoadEvent;
        
        public void LevelClear()
        {
            LevelClearEvent?.Invoke();
        }
        
        public void LoadScene(string sceneName,Action callback = null)
        {
            SceneLoadEvent?.Invoke(sceneName,callback);
        }
        
    }
}
