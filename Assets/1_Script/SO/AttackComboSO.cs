using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public enum EComboState
    {
        None,
        Dash,
        LightAttack,
        PowerAttack,
        AnyAttack// todo : implement this feature
    }
    /// <summary>
    /// note : if intersecting EComboStates have different param, it won't work properly
    /// </summary>
    [Serializable]
    public class ComboData
    {
        [SerializeField] private EComboState comboState;
        [SerializeField] private AnimationParameterSO comboData;
        [SerializeField] private Vector3 comboForce;
        //[SerializeField] private float period;
        public EComboState GetComboState => comboState;
        public AnimationParameterSO GetAnimParam => comboData;
        public Vector3 GetComboForce => comboForce;
        //public float GetPeriod => period;
    }
    [CreateAssetMenu(fileName = "AttackComboSO", menuName = "Scriptable Objects/AttackComboSO")]
    public class AttackComboSO : ScriptableObject
    {
        [SerializeField] private ComboData[] combos;
        public IReadOnlyList<ComboData> GetCombos => combos;
        public bool IsMatch(IReadOnlyList<EComboState> comboStructs, out ComboData result)
        {
            result = null;
            if (comboStructs.Count > combos.Length) return false;
            for (int i = 0; i < comboStructs.Count; i++)
            {
                if (comboStructs[i] != combos[i].GetComboState)
                    return false;
            }
            result = combos[comboStructs.Count - 1];
            return true;
        }
    }
}
