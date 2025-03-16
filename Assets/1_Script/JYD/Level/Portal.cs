using UnityEngine;

namespace Swift_Blade.Level.Portal
{
    public class Portal : MonoBehaviour, IInteractable
    {
        [SerializeField] private SceneManagerSO sceneManager;
        
        [SerializeField] private bool isDefaultPortal;
        
        private ParticleSystem particle;
        [SerializeField] private string sceneName;
        
        
        private void Start()
        {
            particle = GetComponentInChildren<ParticleSystem>();
            particle.Play();
            
            //transform.LookAt(Camera.main.transform);
            
            if (isDefaultPortal)
            {
                particle.Simulate(0);
                particle.Play();
            }
                       
        }
        public void Interact()
        {
            sceneManager.LoadScene(sceneName);
        }

        public void ActivePortal()
        {
            /*particle.Simulate(0);
            particle.Play();*/
        }

        public void SetLoadScene(string _sceneName)
        {
            sceneName = _sceneName;
        }
        
        
        
    }
}
