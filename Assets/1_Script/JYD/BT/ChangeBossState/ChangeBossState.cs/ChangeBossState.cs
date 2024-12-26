using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/Change BossState")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "Change BossState", message: "Change [BossState]", category: "Events", id: "b8051c9b13d36fb0425ddb358dacff49")]
public partial class ChangeBossState : EventChannelBase
{
    public delegate void ChangeBossStateEventHandler(BossState BossState);
    public event ChangeBossStateEventHandler Event; 

    public void SendEventMessage(BossState BossState)
    {
        Event?.Invoke(BossState);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<BossState> BossStateBlackboardVariable = messageData[0] as BlackboardVariable<BossState>;
        var BossState = BossStateBlackboardVariable != null ? BossStateBlackboardVariable.Value : default(BossState);

        Event?.Invoke(BossState);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        ChangeBossStateEventHandler del = (BossState) =>
        {
            BlackboardVariable<BossState> var0 = vars[0] as BlackboardVariable<BossState>;
            if(var0 != null)
                var0.Value = BossState;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as ChangeBossStateEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as ChangeBossStateEventHandler;
    }
}

