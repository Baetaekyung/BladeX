using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/Change State")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "Change State", message: "Change [State]", category: "Events", id: "fee846781191182208499d566fa96462")]
public partial class ChangeBossState : EventChannelBase
{
    public delegate void ChangeStateEventHandler(BossState State);
    public event ChangeStateEventHandler Event; 

    public void SendEventMessage(BossState State)
    {
        Event?.Invoke(State);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<BossState> StateBlackboardVariable = messageData[0] as BlackboardVariable<BossState>;
        var State = StateBlackboardVariable != null ? StateBlackboardVariable.Value : default(BossState);

        Event?.Invoke(State);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        ChangeStateEventHandler del = (State) =>
        {
            BlackboardVariable<BossState> var0 = vars[0] as BlackboardVariable<BossState>;
            if(var0 != null)
                var0.Value = State;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as ChangeStateEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as ChangeStateEventHandler;
    }
}

