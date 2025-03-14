using Unity.Hierarchy;
using UnityEngine;



namespace Swift_Blade.Level.Portal
{
    public class Portal : MonoBehaviour, IInteractable
    {
        [SerializeField] private SceneManagerSO sceneManager;
        [SerializeField] private NodeList nodeList;
        
        [SerializeField] private NodeType type;
        
        public void Interact()
        {
            string nodeName = nodeList.GetNode(type);
            
            sceneManager.LoadScene(nodeName);
        }
    }
}
