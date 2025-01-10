using System;
using System.Collections.Generic;
using UnityEngine;

namespace Swift_Blade
{
    public enum EComboState
    {
        None,
        Dash,
        WeakAttack,
        PowerAttack,
        AnyAttack
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
        [SerializeField] private float period;
        public EComboState GetComboState => comboState;
        public AnimationParameterSO GetAnimParam => comboData;
        public Vector3 GetComboForce => comboForce;
        public float GetPeriod => period;
    }
    [CreateAssetMenu(fileName = "AttackComboSO", menuName = "Scriptable Objects/AttackComboSO")]
    public class AttackComboSO : ScriptableObject
    {
        [SerializeField] private ComboData[] comboes;
        public IReadOnlyList<ComboData> GetComboes => comboes;
        // todo :
        public bool IsMatchFirstIndex(EComboState comboState, out AttackComboSO result)
        {
            result = null;
            //if (comboes[0].GetComboState == comboState)
            //{
            //
            //    return true;
            //}
            return false;
        }
        // todo :
        public bool IsMatch(IReadOnlyList<EComboState> comboStructs, out AttackComboSO result)
        {
            result = null;
            return default;
        }
    }
}
