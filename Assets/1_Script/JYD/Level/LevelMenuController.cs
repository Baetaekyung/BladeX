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
            //random으로 씬 가져오는 코드.

            if (IsVailedScene(sceneName))
            {
                Debug.Log(sceneName);
                levelEvent.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError("유요한 씬이름이 아닙니다!");
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
