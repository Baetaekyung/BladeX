using UnityEngine;
using System;
using System.Collections.Generic;
using Swift_Blade.FSM.States;

namespace Swift_Blade.FSM
{
    public class FiniteStateMachine<StateEnum> 
        where StateEnum : Enum
    {
        public event Action<StateEnum> OnChangeState;

        public State<StateEnum> CurrentState { get; protected set; }
        //public IReadOnlyDictionary<StateEnum, State<StateEnum>> GetStateDictionary => StateDictionary;
        public Dictionary<StateEnum, State<StateEnum>> StateDictionary { get; set; } = new();

        public void AddState(StateEnum type, State<StateEnum> instance) =>  StateDictionary.Add(type, instance);
        public void SetStartState(StateEnum state) => CurrentState = StateDictionary[state];
        public void ChangeState(StateEnum type)
        {
            //Debug.Log(type.ToString());
            try
            {
                OnChangeState?.Invoke(type);
                CurrentState.Exit();
                CurrentState = StateDictionary[type];
                CurrentState.Enter();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        public void UpdateState()
        {
            try
            {
                CurrentState.Current?.Invoke();  
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        } 


        public StateEnum GetState()
        {
            foreach (var kvp in StateDictionary)
            {
                if (kvp.Value == CurrentState)
                    return kvp.Key;
            }

            return default;
        }
    }
}
