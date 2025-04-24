using UnityEngine;

namespace Swift_Blade
{
    public class PlayerVisualController : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private EquipmentListSO equipListSO;

        [SerializeField] private Transform HelmetVisual, BodiesVisual;

        public void EntityComponentAwake(Entity entity)
        {
            
        }

        public void OnParts(string partsName)
        {
            if(equipListSO.equipmentName.TryGetValue(partsName, out var equipmentType))
            {
                if(equipmentType == EquipmentType.Heads)
                {
                    foreach (Transform t in HelmetVisual.transform)
                    {
                        if (t.name == partsName)
                        {
                            Debug.Log("찾았다장비");
                            t.gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    foreach (Transform t in BodiesVisual.transform)
                    {
                        if (t.name == partsName)
                        {
                            Debug.Log("찾았다장비");
                            t.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }

        public void OffParts(string partsName)
        {
            if (equipListSO.equipmentName.TryGetValue(partsName, out var equipmentType))
            {
                if (equipmentType == EquipmentType.Heads)
                {
                    foreach (Transform t in HelmetVisual.transform)
                    {
                        Debug.Log(t.name);
                        if (t.name == partsName)
                        {
                            Debug.Log("찾았다장비");
                            t.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    foreach (Transform t in BodiesVisual.transform)
                    {
                        Debug.Log(t.name);
                        if (t.name == partsName)
                        {
                            Debug.Log("찾았다장비");
                            t.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
