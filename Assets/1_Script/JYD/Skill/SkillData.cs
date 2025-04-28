using System.Collections.Generic;
using Swift_Blade.Pool;
using UnityEngine;

namespace Swift_Blade.Skill
{
    public abstract class SkillData : ScriptableObject, IPlayerEquipable
    {
        public string skillName;
        public Sprite skillIcon;
        [Space(30)]
        public SkillType skillType;
        public StatType statType;
        public ColorType colorType;
        ColorType IPlayerEquipable.GetColor => colorType;
        Sprite IPlayerEquipable.GetSprite => skillIcon;
        string IPlayerEquipable.DisplayName => skillName;


        [Tooltip("비율")] public float colorRatio;
        [Tooltip("확률")][Range(1,100)] public int random;
        [Tooltip("이거보다 높아질순 없음")][Range(1,100)] public int maxRandom;
        
        [TextArea] public string skillDescription;
        
        [Space(40)]
        public PoolPrefabMonoBehaviourSO skillParticle;

        protected PlayerStatCompo statCompo;
        
        public void SetPlayerStatCompo(PlayerStatCompo playerStatCompo)
        {
            statCompo = playerStatCompo;
        }
        
        public virtual void Initialize(){}
        
        public virtual void ResetSkill(){}
        
        public virtual void Render(){}
        
        public virtual void SkillUpdate(Player player, IEnumerable<Transform> targets = null){}
        
        public abstract void UseSkill(Player player, IEnumerable<Transform> targets = null);
        
        protected bool TryUseSkill(int value = 0)
        {
            return Random.Range(0, 100) <= Mathf.Min(maxRandom, random + value);
        }
        
        protected float GetColorRatio()
        {
            if (statCompo == null)
            {
                Debug.LogError("Skill Error : statCompo is null");
                return 0;
            }
            
            return statCompo.GetColorStatValue(colorType) * colorRatio;
        }
    }
}