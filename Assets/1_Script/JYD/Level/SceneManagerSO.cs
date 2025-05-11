using UnityEngine;
using System;
using Swift_Blade.Feeling;
using Swift_Blade.Level;
using UnityEngine.Experimental.GlobalIllumination;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "SceneManager", menuName = "SO/Scene/SceneManager")]
    public class SceneManagerSO : ScriptableObject
    {
        public event Action LevelClearEvent;
        public event Action<string,Action> SceneLoadEvent;
        public event Action SceneEnterEvent;

        public NodeList NodeList;
        public HitStopSO hitStopSO;
        
        public void LevelClear()
        {
            HitStopManager.Instance.EndHitStop();
            HitStopManager.Instance.StartHitStop(hitStopSO);
            
            LevelClearEvent?.Invoke();
        }
        
        public void LoadScene(string sceneName)
        {
            NodeList.RemoveNode(sceneName);
            
            SceneLoadEvent?.Invoke(sceneName,SceneEnterEvent);
        }
        
    }
}
