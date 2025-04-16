using UnityEngine;

namespace Swift_Blade.Level
{
    public class NodeReset : MonoBehaviour
    {
        [SerializeField] private NodeList nodeList;

        public void RestNodeIndex()
        {
            nodeList.RestNodeIndex();
        }
        
    }
}
