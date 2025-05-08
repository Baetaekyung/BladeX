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

        //SO�� �ʿ��Ѱ͵�
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

        [MenuItem("Tools/Create Equipment Editor")]
        public static void ShowWindow()
        {
            GetWindow<EquipmentCreaterEditor>("My Tool");
        }

        void OnGUI()
        {
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
            slotType = (EquipmentSlotType)EditorGUILayout.EnumPopup( slotType);

            GUILayout.Label("\nColorType", EditorStyles.boldLabel);
            colorType = (ColorType)EditorGUILayout.EnumPopup(colorType);

            GUILayout.Label("\nColorAdder", EditorStyles.boldLabel);
            colorAdder = EditorGUILayout.IntField(colorAdder);

            if (GUILayout.Button("\nCreate EquipmentSO"))
            {
                CreateItemRGB();
                Debug.Log("Create SO");
            }
            #endregion
        }

        private void CreateItemRGB()
        {
            string folderPath = "Assets\\08SODatas\\Equipments";
            string assetName = displayName;

            if(slotType == EquipmentSlotType.HEAD)
            {
                folderPath += "\\Head";
            }
            else if (slotType == EquipmentSlotType.ARMOR)
            {
                folderPath += "\\Armor";
            }
            else if (slotType == EquipmentSlotType.WEAPON)
            {
                folderPath += "\\Weapon";
            }
            else if (slotType == EquipmentSlotType.RING)
            {
                folderPath += "\\Ring";
            }
            else if (slotType == EquipmentSlotType.SHOES)
            {
                folderPath += "\\Shoes";
            }

            if (!Directory.Exists(folderPath))
            {
                Debug.Log("forderPath ���µ���");
                return;
            }

            // SO �ν��Ͻ� ����
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
                Debug.LogError("partsName �ʵ� �� ã��");

            field = asset.GetType().GetField("displayName", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field != null)
                field.SetValue(asset, this.displayName);
            else
                Debug.LogError("displayName �ʵ� �� ã��");

            asset.equipmentIcon = this.equipmentIcon;
            asset.slotType = this.slotType;
            asset.colorType = this.colorType;
            asset.colorAdder = this.colorAdder;
            #endregion

            //assetName ���� ����
            string itemFolderPath = Path.Combine(folderPath, assetName);
            if (!Directory.Exists(itemFolderPath))
            {
                Directory.CreateDirectory(itemFolderPath);
                //Debug.Log($"���� ������: {itemFolderPath}");
            }

            //Red, Green, Blue ���� ���� ����
            string[] colorFolders = { "Red", "Green", "Blue" };

            foreach (var color in colorFolders)
            {
                string colorFolderPath = Path.Combine(itemFolderPath, color);
                if (!Directory.Exists(colorFolderPath))
                {
                    Directory.CreateDirectory(colorFolderPath);
                    //Debug.Log($"���� ���� ������: {colorFolderPath}");
                }
            }

            // SO �ν��Ͻ��� ������ RGB ������ ����
            foreach (var color in colorFolders)
            {
                string colorFolderPath = Path.Combine(itemFolderPath, color);

                // ���纻 ����
                EquipmentData assetCopy = ScriptableObject.Instantiate(asset);

                // ���� �̸� ����
                string prefix = color[0].ToString().ToUpper(); // R, G, B

                if(prefix == "R") assetCopy.colorType = ColorType.RED;
                else if (prefix == "G") assetCopy.colorType = ColorType.GREEN;
                else if (prefix == "B") assetCopy.colorType = ColorType.BLUE;

                string prefixAssetname = $"{prefix}_{assetName}";
                string finalAssetName = $"{prefixAssetname}_{"Equip"}.asset";
                // ��ü ���� ���
                string assetPath = Path.Combine(colorFolderPath, finalAssetName);
                string prefabFullPath = Path.Combine(colorFolderPath, $"{prefixAssetname}.prefab");

                // ��� ���ϼ� ���� (������ ���)
                assetPath = assetPath.Replace("\\", "/");
                prefabFullPath = prefabFullPath.Replace("\\", "/");

                // ���� ���� ����
                //Debug.Log($"������(��) SO: {assetPath}");
                AssetDatabase.CreateAsset(assetCopy, assetPath);

                //Debug.Log($"name : {prefabFullPath}");
                CreateOtherAsset(prefixAssetname, prefabFullPath, assetCopy); //��ΰ� ������ (������ �����ؾߵǴµ�)
            }

            // ����
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void CreateOtherAsset(string assetName, string path, EquipmentData equipmentData)
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
                Debug.LogError("equipData �ʵ带 ã�� �� �����ϴ�.");
            }
            Debug.Log(equipDataField);
            //equipData.SetValue();
            //if (!AssetDatabase.IsValidFolder(path))
            //{
            //    Directory.CreateDirectory(path);
            //    AssetDatabase.Refresh();                
            //} gpts Fake Codes

            PrefabUtility.SaveAsPrefabAsset(go, path);
            Debug.Log($"������ ������: {path}");

            Object.DestroyImmediate(go);
        }
    }
}
