using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public enum EquipmentType
    {
        Heads,
        Bodies
    }


    [CreateAssetMenu(fileName = "EquipmentListSO", menuName = "SO/EquipmentListSO")]
    public class EquipmentListSO : ScriptableObject
    {
        public SerializableDictionary<string, EquipmentType> equipmentName;
    }
}
