using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public enum TagState
    {
        NONE = 0,
        MIN,
        MIDDLE,
        MAX,
        ALL = 99
    }

    public abstract class TagEffectBase : MonoBehaviour
    {
        [SerializeField] protected int minTagCount    = 2;
        [SerializeField] protected int middleTagCount = 3;
        [SerializeField] protected int maxTagCount    = 5;

        private bool _hasMinEffect      = false;
        private bool _hasMiddleEffect   = false;
        private bool _hasMaxEffect      = false;

        [Space]
        [TextArea] public string tagEffectDescription;

        public TagState GetTagState(int tagCount)
        {
            if (tagCount >= maxTagCount)
            {
                return TagState.MAX;
            }
            else if (tagCount >= middleTagCount)
            {
                return TagState.MIDDLE;
            }
            else if (tagCount >= minTagCount)
            {
                return TagState.MIN;
            }
            else
            {
                return TagState.NONE;
            }
        }

        public void EnableTagEffect(int tagCount)
        {
            TagState newTagState = GetTagState(tagCount);
            
            switch (newTagState)
            {
                //Min
                case TagState.MIN:
                    OffEffectMaxTag();
                    OffEffectMiddleTag();

                    EffectMinTag();
                    break;
                //Middle
                case TagState.MIDDLE:
                    OffEffectMinTag();
                    OffEffectMiddleTag();

                    EffectMiddleTag();
                    break;
                //Max
                case TagState.MAX:
                    OffEffectMinTag();
                    OffEffectMiddleTag();

                    EffectMaxTag();
                    break;
                //Else
                default:
                    break;
            }
        }

        public virtual void EffectMinTag() 
        {
            if (_hasMinEffect)
                return;

            _hasMinEffect = true;
        }

        public virtual void EffectMiddleTag() 
        {
            if (_hasMiddleEffect)
                return;

            _hasMiddleEffect = true;
        }

        public virtual void EffectMaxTag()
        {
            if (_hasMaxEffect)
                return;

            _hasMaxEffect = true;
        }

        public virtual void OffEffectMinTag() 
        {
            if (_hasMinEffect == false)
                return;

            _hasMinEffect = false;
        }

        public virtual void OffEffectMiddleTag()
        {
            if (_hasMiddleEffect == false)
                return;

            _hasMiddleEffect = false;
        }

        public virtual void OffEffectMaxTag()
        {
            if (_hasMaxEffect == false)
                return;

            _hasMaxEffect = false;
        }

        public abstract bool IsValidToEnable(int tagCount);
    }
}
