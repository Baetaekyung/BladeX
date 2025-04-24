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
        private readonly Dictionary<StateEnum, State<StateEnum>> stateDictionary;

        public FiniteStateMachine()
        {
            stateDictionary = new Dictionary<StateEnum, State<StateEnum>>();
        }

        public void AddState(StateEnum type, State<StateEnum> instance) => stateDictionary.Add(type, instance);
        public void SetStartState(StateEnum state) => CurrentState = stateDictionary[state];
        public void ChangeState(StateEnum type)
        {
            OnChangeState?.Invoke(type);
            CurrentState.Exit();
            CurrentState = stateDictionary[type];
            CurrentState.Enter();
        }
        public void UpdateState()
        {
            CurrentState.Current?.Invoke();
        }
        public void Exit()
        {
            CurrentState.Exit();
        }
        public StateEnum GetState()
        {
            foreach (KeyValuePair<StateEnum, State<StateEnum>> kvp in stateDictionary)
            {
                if (kvp.Value == CurrentState)
                    return kvp.Key;
            }

            return default;
        }
    }
}
