using System;
using System.Collections.Generic;
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
            Player player = entity as Player;
            foreach (TagEffectBase tagEff in tagEffects.Values)
            {
                tagEff.Initialize(player);
            }
        }
        private void InitializeTagCount()
        {
            _tagCounts = new Dictionary<EquipmentTag, int>();

            foreach (object tag in Enum.GetValues(typeof(EquipmentTag)))
            {
                _tagCounts.Add((EquipmentTag)tag, 0);
            }
        }
        public void AddTagCount(EquipmentTag equipTag)
        {
            if (_tagCounts.ContainsKey(equipTag))
            {
                TagEffectUpdate(equipTag, true);
            }
            else
            {
                Debug.LogWarning($"Can't Initialize tag, Tag [{equipTag}]");
            }

            Debug.Log($"Tag [{equipTag}] count is {_tagCounts[equipTag]}");
        }

        public void RemoveTagCount(EquipmentTag equipTag)
        {
            if (_tagCounts.ContainsKey(equipTag))//TryGetValue(equipTag, out var count))
            {
                //if (count == 0)
                //    return;

                TagEffectUpdate(equipTag, false);
            }
            else
            {
                Debug.LogWarning($"Tag not initialized, Tag [{equipTag}]");
            }
        }
        private void TagEffectUpdate(EquipmentTag equipTag, bool isIncreasing)
        {
            TagEffectBase tagEffect = tagEffects[equipTag];
            int tagCount = isIncreasing ?
                ++_tagCounts[equipTag] :
                --_tagCounts[equipTag];

            tagEffect.TagEffect(tagCount, isIncreasing);
        }
        //private void AddTagEffect(EquipmentTag equipTag)
        //{
        //    #region Validation

        //    ////isnt this an error
        //    ////this never happens
        //    //if (_tagCounts.ContainsKey(equipTag) == false)
        //    //    return;

        //    //if (tagEffects.ContainsKey(equipTag) == false)
        //    //    return;

        //    #endregion

        //    TagEffectBase tagEffect = tagEffects[equipTag];
        //    int tagCount = ++_tagCounts[equipTag];
        //    bool effectFlag = tagEffect.IsTagCountValid(tagCount);

        //    if (effectFlag)
        //    {
        //        //tagEffect.TagEffect(tagCount);
        //    }
        //}
        //private void RemoveTagEffect(EquipmentTag equipTag)
        //{
        //    #region Validation

        //    ////isnt this an error
        //    //if (_tagCounts.ContainsKey(equipTag) == false)
        //    //    return;

        //    //if (tagEffects.ContainsKey(equipTag) == false)
        //    //    return;

        //    #endregion

        //    TagEffectBase tagEffect = tagEffects[equipTag];
        //    int tagCount = --_tagCounts[equipTag];
        //    bool effectFlag = tagEffect.IsTagCountValid(tagCount);

        //    if (effectFlag)
        //    {
        //        //tagEffect.TagEffect(tagCount);
        //    }
        //    else
        //    {
        //        tagEffect.DisableTagEffect();
        //    }
        //}
    }
}
