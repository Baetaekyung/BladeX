using UnityEngine;

namespace Swift_Blade
{
    public class PlayerVisualController : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private EquipmentListSO equipListSO;

        [SerializeField] private Transform HelmetVisual, HelmetVisual2, BodiesVisual, HipVisual;
        [SerializeField] private Transform LeftShoesVisuals, RightShoesVisuals;
        [SerializeField] private Transform DefaultTorso;

        [SerializeField] private Transform[] Visuals;

        private bool objIsTorso = false;

        public void EntityComponentAwake(Entity entity)
        {
            
        }

        public void OnParts(string equipment)
        {
            GameObject go = null;
            if (equipListSO.equipmentList.TryGetValue(equipment, out var partsName))
            {
                go = GetVisualObj(partsName);
                if (go is null)
                {
                    string offsetR = $"Chr_LegRight_Male_{partsName}";
                    string offsetL = $"Chr_LegLeft_Male_{partsName}";
                    //신발은 파츠네임에 숫자만기입

                    go = RightShoesVisuals.Find(offsetR).gameObject;
                    go.SetActive(true);
                    go = LeftShoesVisuals.Find(offsetL).gameObject;
                    go.SetActive(true);
                }
                else
                {
                    go.SetActive(true);
                }

                if(objIsTorso)
                {
                    DefaultTorso.gameObject.SetActive(false);
                }
            }
        }

        public void OffParts(string equipment)
        {

            GameObject go = null;
            if (equipListSO.equipmentList.TryGetValue(equipment, out var partsName))
            {
                go = GetVisualObj(partsName);
                if (go is null)
                {
                    string offsetR = $"Chr_LegRight_Male_{partsName}";
                    string offsetL = $"Chr_LegLeft_Male_{partsName}";
                    //신발은 파츠네임에 숫자만기입

                    go = RightShoesVisuals.Find(offsetR).gameObject;
                    go.SetActive(false);
                    go = LeftShoesVisuals.Find(offsetL).gameObject;
                    go.SetActive(false);
                }
                else
                {
                    go.SetActive(false);
                }

                if (objIsTorso)
                {
                    DefaultTorso.gameObject.SetActive(true);
                }
            }
        }

        private GameObject GetVisualObj(string partsName)
        {
            objIsTorso = false;

            GameObject go = null;

            foreach(Transform parentVis in Visuals)
            {
                objIsTorso = (parentVis.name == "Male_03_Torso");

                go = GetFindObj(parentVis, partsName);
                if(go != null)
                {
                    Debug.Log($"exist on {parentVis}");
                    break;
                }
            }

            return go;

        
            //if(trm == null)

            //Debug.Log("슈에 있습니다");
            //return null;

            //if error by shoe part? fixing like this style at FixingErrorTime
        }

        private GameObject GetFindObj(Transform visParent, string partsName)
        {
            Transform trm = visParent.Find(partsName);
            if (trm != null)
            {
                Debug.Log($"{visParent} 에 있습니다");
                SetOffVisuals(visParent);

                return trm.gameObject;
            }

            Debug.Log("겟파인트오브젝트인데 널이뜨네요");
            return null;
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
