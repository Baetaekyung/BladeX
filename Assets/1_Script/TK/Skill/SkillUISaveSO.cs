using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "SkillSaveSO", menuName = "SO/SkillSaveData")]
    public class SkillUISaveSO : ScriptableObject
    {
        //여기서 스킬 데이터들 관리해서 저장하자.
        public List<SkillUIDataSO> invUiData;
        public List<SkillUIDataSO> skillSlotUiData;
        
        public SkillUISaveSO Clone() => Instantiate(this);
    }
}
