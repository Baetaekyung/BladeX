using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Serialization;
using Action = System.Action;

namespace Swift_Blade
{
    [Serializable]
    public struct TalkingData
    {
        public string talker;
        [TextArea]
        public string dialogueMessage;
    }

    [CreateAssetMenu(fileName = "Dialog_", menuName = "SO/Dialog/DialogData")]
    public class DialogueDataSO : ScriptableObject
    {
        public List<TalkingData> dialougueDatas = new();
        [Tooltip("글자가 나타나는 속도")] 
        public float dialogueWaitTime;

        public List<DialogueEventSO> dialogueEvent = new();
    }
}
