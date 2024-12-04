using UnityEngine;
using System;
using System.Collections.Generic;
using Swift_Blade.FSM.States;

namespace Swift_Blade.FSM
{
    public class FiniteStateMachine<StateEnum> 
        where StateEnum : Enum
    {
        public State<StateEnum> CurrentState { get; protected set; }
        public Dictionary<StateEnum, State<StateEnum>> StateDictionary { get; } = new();

        public void AddState(StateEnum type, State<StateEnum> instance) =>  StateDictionary.Add(type, instance);
        public void SetStartState(StateEnum state) => CurrentState = StateDictionary[state];
        public void ChangeState(StateEnum type)
        {
            CurrentState.Exit();
            CurrentState = StateDictionary[type];
            CurrentState.Enter();
        }
        public void UpdateState() => CurrentState.Current?.Invoke();
    }
}
