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
                                    
            if (isDefaultPortal)
            {
                particle.Simulate(0);
                particle.Play();
            }
            else
            {
                Vector3 direction = Camera.main.transform.position - transform.position;
                direction.y = 0; 
                transform.rotation = Quaternion.LookRotation(direction);
            }
                       
        }
        public void Interact()
        {
            sceneManager.LoadScene(sceneName);
        }

        public void ActivePortal()
        {
            particle.Simulate(0);
            particle.Play();
        }

        public void SetScene(string _sceneName)
        {
            sceneName = _sceneName;
        }
        
        
        
    }
}
