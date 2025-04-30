using UnityEngine;

namespace Swift_Blade
{
    public class PlayerVisualController : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private EquipmentListSO equipListSO;

        [SerializeField] private Transform HelmetVisual, HelmetVisual2, BodiesVisual;

        public void EntityComponentAwake(Entity entity)
        {
            
        }

        public void OnParts(string equipment)
        {
            if(equipListSO.equipmentList.TryGetValue(equipment, out var partsName))
            {
                GameObject go = null;
                if (HelmetVisual.Find(partsName) is not null)
                {
                    //Debug.Log("«Ô∏‰ø° ¿÷Ω¿¥œ¥Ÿ");
                    go = HelmetVisual.Find(partsName).gameObject;
                }
                else if (HelmetVisual2.Find(partsName) is not null)
                {
                    //Debug.Log("«Ô∏‰2ø° ¿÷Ω¿¥œ¥Ÿ");
                    go = HelmetVisual2.Find(partsName).gameObject;
                }
                else
                {
                    //Debug.Log("∞©ø ø° ¿÷Ω¿¥œ¥Ÿ");
                    go = BodiesVisual.Find(partsName).gameObject;
                }
                if (go == null) return;
                go.SetActive(true);
            }
        }

        public void OffParts(string equipment)
        {
            if (equipListSO.equipmentList.TryGetValue(equipment, out var partsName))
            {
                GameObject go = null;
                if (HelmetVisual.Find(partsName) is not null)
                {
                    go = HelmetVisual.Find(partsName).gameObject;
                }
                else if (HelmetVisual2.Find(partsName) is not null)
                {
                    Debug.Log("«Ô∏‰2ø° ¿÷Ω¿¥œ¥Ÿ");
                    go = HelmetVisual2.Find(partsName).gameObject;
                }
                else
                {
                    go = BodiesVisual.Find(partsName).gameObject;
                }

                go.SetActive(false);
            }
        }

        private void SetOffVisuals(Transform t)
        {
            foreach(Transform child in t)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
