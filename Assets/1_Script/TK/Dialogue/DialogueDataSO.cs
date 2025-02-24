using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "Dialog_", menuName = "SO/Dialog/DialogData")]
    public class DialogueDataSO : ScriptableObject
    {
        [Tooltip("말하는 사람")]
        public string talker;
        [TextArea]
        public List<string> dialogueMessage = new();
        [Tooltip("글자가 나타나는 속도")] 
        public float dialogueSpeed;

        public List<DialogueEventSO> dialogueEvent = new();
    }
}
