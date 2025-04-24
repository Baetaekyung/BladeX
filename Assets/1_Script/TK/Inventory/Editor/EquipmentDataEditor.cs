using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Swift_Blade
{
    public class EquipmentDataEditor : EditorWindow
    {
        private const string DEFAULT_SAVE_PATH = "Assets/08SODatas/Equipments/";

        private EquipmentSlotType  _slotType;
        private List<EquipmentTag> _tags = new List<EquipmentTag>();
        private EquipmentRarity    _rarity;
        private ColorType          _color;

        private string _partName;
        private Sprite _equipmentIcon;
        private int    _colorAdder;

        [MenuItem("Tools/EquipDataSO Generator")]
        public static void ShowWindow()
        {
            var window = GetWindow<EquipmentDataEditor>();

            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("EquipDataSO Generator");
            GUILayout.Space(10);

            #region Path

            string finalPath = string.Empty;

            EditorGUILayout.LabelField("장비를 저장할 파일의 이름을 적으세요.");

            string savePath = string.Empty;
            savePath = EditorGUILayout.TextField(savePath);

            EditorGUILayout.LabelField("장비의 이름을 적으세요.");
            string equipName = string.Empty;
            equipName = EditorGUILayout.TextField(equipName);

            finalPath = DEFAULT_SAVE_PATH + savePath + "/" + equipName + ".asset";

            #endregion

            #region Set data

            _slotType = (EquipmentSlotType)EditorGUILayout.EnumPopup(_slotType);

            int tagFieldCount = 0;
            tagFieldCount = EditorGUILayout.IntField(tagFieldCount);

            for (int i = 0; i < tagFieldCount; i++)
            {
                //_tags[i]
            }

            EquipmentData equipData = CreateInstance<EquipmentData>();
            equipData.rarity = _rarity;

            #endregion

            //AssetDatabase.CreateAsset(obj, finalPath);
            AssetDatabase.SaveAssets();
        }
    }
}
