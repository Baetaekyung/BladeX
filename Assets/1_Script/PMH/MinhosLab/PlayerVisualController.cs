using UnityEngine;

namespace Swift_Blade
{
    public class PlayerVisualController : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private SerializableDictionary<string, GameObject> equipVisuals;

        public void EntityComponentAwake(Entity entity)
        {
            
        }

        public void OnParts(string partsName)
        {
            foreach(GameObject part in equipVisuals.Values)
            {
                part.SetActive(false);
            }

            if(equipVisuals.TryGetValue(partsName, out var equipVisual))
            {
                equipVisual.SetActive(true);
            }
        }

        public void OffParts(string partsName)
        {
            if (equipVisuals.TryGetValue(partsName, out var equipVisual))
            {
                equipVisual.SetActive(false);
            }
        }
    }
}
