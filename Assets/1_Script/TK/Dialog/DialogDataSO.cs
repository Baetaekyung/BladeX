using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

namespace Swift_Blade
{
    [CreateAssetMenu(fileName = "Dialog_", menuName = "SO/Dialog/DialogData")]
    public class DialogDataSO : ScriptableObject
    {
        [Tooltip("말하는 사람")]
        public string talker;
        [TextArea] 
        public List<string> dialogMessage = new();
        [Tooltip("글자가 나타나는 속도")] 
        public float dialogSpeed;

        public List<DialogEventSO> dialogEvent = new();
    }
}
