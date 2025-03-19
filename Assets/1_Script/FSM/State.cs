using UnityEngine;
using System;
namespace Swift_Blade.FSM.States
{
    public abstract class State<StateEnum>
        where StateEnum : Enum
    {
        public Action Current { get; private set; }
        private readonly FiniteStateMachine<StateEnum> fsm;
        protected FiniteStateMachine<StateEnum> GetOwnerFsm => fsm;
        public State(FiniteStateMachine<StateEnum> stateMachine)
        {
            fsm = stateMachine;
            Current = Enter;
        }
        public virtual void Enter()
        {
            Current = Update;
        }
        public virtual void Update()
        {
        }

        public virtual void Exit()
        {
        }
    }
}
