using UnityEngine;

namespace Swift_Blade.Level.Portal
{
    public class Portal : MonoBehaviour, IInteractable
    {
        [SerializeField] private SceneManagerSO sceneManager;
        [SerializeField] private string firstSceneName;
        
        public void Interact()
        {
            sceneManager.LoadScene(firstSceneName);
        }
                
    }
}
