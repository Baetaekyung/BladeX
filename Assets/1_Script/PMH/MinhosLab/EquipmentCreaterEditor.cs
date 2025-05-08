using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security;
using UnityEditor;
using UnityEngine;

namespace Swift_Blade
{
    public class EquipmentCreaterEditor : EditorWindow
    {
        //private string message = "Hello, Unity!";

        //SO에 필요한것들
        public SerializableDictionary<StatType, float> statModifier = new();

        public EquipmentTag tag1;
        public EquipmentTag tag2;
        public EquipmentTag tag3;

        public EquipmentRarity rarity;

        [SerializeField] private string partsName;
        [SerializeField] private string displayName;

        [HideInInspector]
        public string itemSerialCode;
        public Sprite equipmentIcon;

        public EquipmentSlotType slotType;
        public ColorType colorType;
        public int colorAdder;

        //ItemImage = sprite
        public string description;
        //ItemName = displayName;
        public ItemType itemType;
        public ItemObject itemObject;
        //EquipmentData

        //스크롤
        private Vector2 scrollPos;

        [MenuItem("Tools/Create Equipment Editor")]
        public static void ShowWindow()
        {
            GetWindow<EquipmentCreaterEditor>("My Tool");
        }

        void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            #region SetValues
            GUILayout.Label("Stats", EditorStyles.boldLabel);
            foreach (StatType statType in System.Enum.GetValues(typeof(StatType)))
            {
                if (!statModifier.ContainsKey(statType))
                {
                    statModifier.Add(statType, 0);
                }
                statModifier[statType] = EditorGUILayout.FloatField(statType.ToString(), statModifier[statType]);
            }

            GUILayout.Label("\nTags", EditorStyles.boldLabel);
            tag1 = (EquipmentTag)EditorGUILayout.EnumPopup(tag1);
            tag2 = (EquipmentTag)EditorGUILayout.EnumPopup(tag2);
            tag3 = (EquipmentTag)EditorGUILayout.EnumPopup(tag3);

            GUILayout.Label("\nRarity", EditorStyles.boldLabel);
            rarity = (EquipmentRarity)EditorGUILayout.EnumPopup(rarity);

            GUILayout.Label("\nPartName", EditorStyles.boldLabel);
            partsName = EditorGUILayout.TextField("", partsName);

            GUILayout.Label("\nDisplayName", EditorStyles.boldLabel);
            displayName = EditorGUILayout.TextField("", displayName);

            GUILayout.Label("\nEquipmentIcon", EditorStyles.boldLabel);
            equipmentIcon = (Sprite)EditorGUILayout.ObjectField(equipmentIcon, typeof(Sprite), true);

            GUILayout.Label("\nSlotType", EditorStyles.boldLabel);
            slotType = (EquipmentSlotType)EditorGUILayout.EnumPopup(slotType);

            GUILayout.Label("\nColorType", EditorStyles.boldLabel);
            colorType = (ColorType)EditorGUILayout.EnumPopup(colorType);

            GUILayout.Label("\nColorAdder\n", EditorStyles.boldLabel);
            colorAdder = EditorGUILayout.IntField(colorAdder);

            // ItemDataSO를 위한
            GUILayout.Label("\nDescription", EditorStyles.boldLabel);
            description = EditorGUILayout.TextField("Description", description);

            GUILayout.Label("\nSetItemType", EditorStyles.boldLabel);
            itemType = (ItemType)EditorGUILayout.EnumPopup(itemType);


            if (GUILayout.Button("\nCreate EquipmentSO"))
            {
                CreateItemRGB();
                Debug.Log("Create SO");
            }
            #endregion

            EditorGUILayout.EndScrollView();
        }

        private void CreateItemRGB()
        {
            string folderPath = "Assets\\08SODatas\\Equipments";
            string assetName = displayName;

            if(slotType == EquipmentSlotType.HEAD)
            {
                folderPath += "\\Heads";
            }
            else if (slotType == EquipmentSlotType.ARMOR)
            {
                folderPath += "\\Bodies";
            }
            else if (slotType == EquipmentSlotType.WEAPON)
            {
                folderPath += "\\Weapon";
            }
            else if (slotType == EquipmentSlotType.RING)
            {
                folderPath += "\\Rings";
            }
            else if (slotType == EquipmentSlotType.SHOES)
            {
                folderPath += "\\Shoes";
            }

            if (!Directory.Exists(folderPath))
            {
                Debug.Log("forderPath 없는데요");
                return;
            }

            // SO 인스턴스 생성
            #region SOInstance
            EquipmentData asset = ScriptableObject.CreateInstance<EquipmentData>();
            asset.statModifier = new SerializableDictionary<StatType, float>();


            #region Stat
            foreach (var stat in statModifier)
            {
                if (stat.Value != 0)
                {
                    asset.statModifier.Add(stat.Key, stat.Value);
                }
            }
            #endregion
            #region Tags
            int tagCount = 0;
            asset.tags = new List<EquipmentTag>(3);
            if (tag1 != EquipmentTag.NONE)
            {
                tagCount++;
                asset.tags.Add(tag1);
            }
            if (tag2 != EquipmentTag.NONE)
            {
                tagCount++;
                asset.tags.Add(tag2);
            }
            if (tag3 != EquipmentTag.NONE)
            {
                tagCount++;
                asset.tags.Add(tag3);
            }

            #endregion

            asset.rarity = this.rarity;

            var field = asset.GetType().GetField("partsName", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field != null)
                field.SetValue(asset, this.partsName);
            else
                Debug.LogError("partsName 필드 못 찾음");

            field = asset.GetType().GetField("displayName", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field != null)
                field.SetValue(asset, this.displayName);
            else
                Debug.LogError("displayName 필드 못 찾음");

            asset.equipmentIcon = this.equipmentIcon;
            asset.slotType = this.slotType;
            asset.colorType = this.colorType;
            asset.colorAdder = this.colorAdder;
            #endregion

            //assetName 폴더 생성
            string itemFolderPath = Path.Combine(folderPath, assetName);
            if (!Directory.Exists(itemFolderPath))
            {
                Directory.CreateDirectory(itemFolderPath);
                //Debug.Log($"폴더 생성됨: {itemFolderPath}");
            }

            //Red, Green, Blue 하위 폴더 생성
            string[] colorFolders = { "Red", "Green", "Blue" };

            foreach (var color in colorFolders)
            {
                string colorFolderPath = Path.Combine(itemFolderPath, color);
                if (!Directory.Exists(colorFolderPath))
                {
                    Directory.CreateDirectory(colorFolderPath);
                    //Debug.Log($"서브 폴더 생성됨: {colorFolderPath}");
                }
            }

            // SO 인스턴스를 각각의 RGB 폴더에 저장
            foreach (var color in colorFolders)
            {
                string colorFolderPath = Path.Combine(itemFolderPath, color);

                // 복사본 생성
                EquipmentData assetCopy = ScriptableObject.Instantiate(asset);

                // 파일 이름 설정
                string prefix = color[0].ToString().ToUpper(); // R, G, B

                if(prefix == "R") assetCopy.colorType = ColorType.RED;
                else if (prefix == "G") assetCopy.colorType = ColorType.GREEN;
                else if (prefix == "B") assetCopy.colorType = ColorType.BLUE;

                string prefixAssetname = $"{prefix}_{assetName}";
                string finalAssetName = $"{prefixAssetname}_{"Equip"}.asset";
                string soName = $"{prefixAssetname}.asset";
                // 전체 저장 경로
                string prefabFullPath = Path.Combine(colorFolderPath, $"{prefixAssetname}.prefab"); //Prefab
                string assetPath = Path.Combine(colorFolderPath, finalAssetName); //SO
                string soFullPath = Path.Combine(colorFolderPath, soName); //SO

                // 경로 통일성 보장 (슬래시 사용)
                assetPath = assetPath.Replace("\\", "/");
                prefabFullPath = prefabFullPath.Replace("\\", "/");

                // 실제 에셋 생성
                //Debug.Log($"생성된(될) SO: {assetPath}");
                AssetDatabase.CreateAsset(assetCopy, assetPath);

                //Debug.Log($"name : {prefabFullPath}");
                GameObject prefab = CreateEquipmentPrefab(prefixAssetname, prefabFullPath, assetCopy); //경로가 파일임 (폴더에 생성해야되는데)

                CreateItemDataSO(prefixAssetname, soFullPath, assetCopy, prefab.GetComponent<StatEquipment>());
            }

            // 갱신
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private GameObject CreateEquipmentPrefab(string assetName, string path, EquipmentData equipmentData)
        {
            GameObject go = new GameObject(assetName);

            StatEquipment statEquipment = go.AddComponent<StatEquipment>();

            var baseType = typeof(StatEquipment).BaseType;
            var equipDataField = baseType?.GetField("equipData", BindingFlags.Instance | BindingFlags.NonPublic);

            if (equipDataField != null)
            {
                equipDataField.SetValue(statEquipment, equipmentData);
            }
            else
            {
                Debug.LogError("equipData 필드를 찾을 수 없습니다.");
            }
            Debug.Log(equipDataField);
            //equipData.SetValue();
            //if (!AssetDatabase.IsValidFolder(path))
            //{
            //    Directory.CreateDirectory(path);
            //    AssetDatabase.Refresh();                
            //} gpts Fake Codes

            GameObject valueObject = PrefabUtility.SaveAsPrefabAsset(go, path);
            Debug.Log($"프리팹 생성됨: {path}");

            Object.DestroyImmediate(go);

            return valueObject;
        }

        private void CreateItemDataSO(string assetName, string path, EquipmentData so, StatEquipment prefab)
        {
            // ItemDataSO 인스턴스 생성
            ItemDataSO itemData = ScriptableObject.CreateInstance<ItemDataSO>();

            // 데이터 채워넣기
            itemData.itemName = displayName;
            itemData.itemType = itemType;
            itemData.description = description;
            itemData.equipmentData = so;
            itemData.itemImage = equipmentIcon;
            itemData.itemObject = prefab;

            // 에셋 생성
            AssetDatabase.CreateAsset(itemData, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"아이템 SO 생성 완료: {path}");
        }
    }
}
