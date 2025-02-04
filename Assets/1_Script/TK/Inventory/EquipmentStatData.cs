using System;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "SO/Equipment/EquipmentData")]
    public class EquipmentStatData : ScriptableObject
    {
        public string itemId; //Stat�� Add ���ذ��� �ĺ� �� �� �ְ��ϴ� ID
        public SerializableDictionary<StatType, float> statAddValues = new SerializableDictionary<StatType, float>();
        public Sprite icon; //�� ������ ���߿� ��ħ
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            if (itemId != string.Empty) return;
            
            itemId = Guid.NewGuid().ToString();
        }

#endif
    }
}
