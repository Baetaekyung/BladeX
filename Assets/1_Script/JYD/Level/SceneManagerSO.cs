using Swift_Blade.Feeling;
using Swift_Blade.Level;
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
        
        public NodeList NodeList;
        public HitStopSO hitStopSO;

        private void OnEnable()
        {
            SceneEnterEvent += NodeList.IncreaseNodeIndex;
            
            NodeList.AddGameClearEvent(() =>
            {
                LoadScene("Ending");
            });
        }
        
        private void OnDisable()
        {
            SceneEnterEvent -= NodeList.IncreaseNodeIndex;
        }
        
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
        
        public NodeList GetNodeList() => NodeList;
        
                
    }
}
