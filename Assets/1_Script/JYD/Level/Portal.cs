using UnityEngine;

namespace Swift_Blade.Level.Portal
{
    public class Portal : MonoBehaviour, IInteractable
    {
        [SerializeField] private SceneManagerSO sceneManager;
        [SerializeField] private NodeList nodeList;
        
        [SerializeField] private NodeType type;
        
        private ParticleSystem particle;

        private void Start()
        {
            particle = GetComponent<ParticleSystem>();
        }

        public void Interact()
        {
            string nodeName = nodeList.GetNode(type);
            sceneManager.LoadScene(nodeName);
        }

        public void ActivePortal()
        {
            particle.Simulate(0);
            particle.Play();
        }
        
    }
}
