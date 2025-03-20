using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "SkillSaveSO", menuName = "SO/SkillSaveData")]
    public class SkillUISaveSO : ScriptableObject
    {
        //���⼭ ��ų �����͵� �����ؼ� ��������.
        public List<SkillUIDataSO> invUiData;
        public List<SkillUIDataSO> skillSlotUiData;
        
        public SkillUISaveSO Clone() => Instantiate(this);
    }
}
