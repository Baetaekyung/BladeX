using UnityEngine;
using UnityEngine.SceneManagement;

namespace Swift_Blade.Level
{
    
    public class LevelMenuController : MonoBehaviour
    {
        public SceneManagerSO levelEvent;
        [SerializeField] private string sceneName;
        
        public void NextScene()
        {
            //random���� �� �������� �ڵ�.

            if (IsVailedScene(sceneName))
            {
                Debug.Log(sceneName);
                levelEvent.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError("������ ���̸��� �ƴմϴ�!");
            }
            
        }
        
        private bool IsVailedScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
                return false;
            
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string scenePath = System.IO.Path.GetFileNameWithoutExtension(path);
                if (scenePath == sceneName)
                    return true;
            }
            
            return false;
        }
        
    }
}
