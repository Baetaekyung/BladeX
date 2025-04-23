using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerTagCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private SerializableDictionary<EquipmentTag, TagEffectBase> tagEffects = new();

        private Dictionary<EquipmentTag, int> _tagCounts; //How many tags remain?

        public void EntityComponentAwake(Entity entity)
        {
            InitializeTagCount();
        }

        private void InitializeTagCount()
        {
            _tagCounts = new Dictionary<EquipmentTag, int>();

            foreach (var tag in Enum.GetValues(typeof(EquipmentTag)))
                _tagCounts.Add((EquipmentTag)tag, 0);
        }

        public void AddTagCount(EquipmentTag equipTag)
        {
            if (_tagCounts.ContainsKey(equipTag))
                _tagCounts[equipTag]++;
            else
                Debug.LogWarning($"Can't Initialize tag, Tag [{equipTag}]");

            EnableTagEffect(equipTag);

            Debug.Log($"Tag [{equipTag}] count is {_tagCounts[equipTag]}");
        }

        public void RemoveTagCount(EquipmentTag equipTag)
        {
            if (_tagCounts.TryGetValue(equipTag, out var count))
            {
                if (count == 0)
                    return;

                _tagCounts[equipTag]--;
                EnableTagEffect(equipTag);
            }
            else
                Debug.LogWarning($"Tag not initialized, Tag [{equipTag}]");
        }

        private void EnableTagEffect(EquipmentTag equipTag)
        {
            #region Validation

            if (_tagCounts.ContainsKey(equipTag) == false)
                return;

            if (tagEffects.ContainsKey(equipTag) == false)
                return;

            #endregion

            var currentTagCount = _tagCounts[equipTag];
            var isValidTag      = tagEffects[equipTag].IsValidToEnable(currentTagCount);

            if (isValidTag)
            {
                tagEffects[equipTag].EnableTagEffect(currentTagCount);
            }
        }
    }
}
