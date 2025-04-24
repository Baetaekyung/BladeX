using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Swift_Blade
{
    public class PlayerTagCompo : MonoBehaviour, IEntityComponent, IEntityComponentStart
    {
        [SerializeField] private SerializableDictionary<EquipmentTag, TagEffectBase> tagEffects = new();

        private Dictionary<EquipmentTag, int> _tagCounts; //How many tags remain?

        public void EntityComponentAwake(Entity entity)
        {
            InitializeTagCount();
        }

        public void EntityComponentStart(Entity entity)
        {
            foreach (var tagEff in tagEffects.Values)
            {
                tagEff.Initialize(entity as Player);
            }
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
                TryAddTagEffect(equipTag);
            else
                Debug.LogWarning($"Can't Initialize tag, Tag [{equipTag}]");

            Debug.Log($"Tag [{equipTag}] count is {_tagCounts[equipTag]}");
        }

        public void RemoveTagCount(EquipmentTag equipTag)
        {
            if (_tagCounts.TryGetValue(equipTag, out var count))
            {
                if (count == 0)
                    return;

                TryRemoveTagEffect(equipTag);
            }
            else
                Debug.LogWarning($"Tag not initialized, Tag [{equipTag}]");
        }

        private void TryAddTagEffect(EquipmentTag equipTag)
        {
            #region Validation

            if (_tagCounts.ContainsKey(equipTag) == false)
                return;

            if (tagEffects.ContainsKey(equipTag) == false)
                return;

            #endregion

            //현재 진행 중인 효과 있으면 지우고
            tagEffects[equipTag].DisableTagEffect(_tagCounts[equipTag]);
            _tagCounts[equipTag]++;

            var currentTagCount = _tagCounts[equipTag];
            var isValidTag      = tagEffects[equipTag].IsValidToEnable(currentTagCount);

            //새로운 효과로 적용
            if (isValidTag)
            {
                tagEffects[equipTag].EnableTagEffect(currentTagCount);
            }
        }

        private void TryRemoveTagEffect(EquipmentTag equipTag)
        {
            #region Validation

            if (_tagCounts.ContainsKey(equipTag) == false)
                return;

            if (tagEffects.ContainsKey(equipTag) == false)
                return;

            #endregion

            var currentTagCount = _tagCounts[equipTag];

            tagEffects[equipTag].DisableTagEffect(currentTagCount);
            _tagCounts[equipTag]--;

            //지운 후에 줄어든 태그 효과로 실행
            tagEffects[equipTag].EnableTagEffect(_tagCounts[equipTag]);
        }
    }
}
