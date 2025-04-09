using Swift_Blade.Level.Door;
using UnityEngine;

namespace Swift_Blade
{
    public class Memos : MonoBehaviour, IInteractable
    {
        [SerializeField] private string m_talker;
        [SerializeField] private string m_scripts;

        private DialogueDataSO m_dialogueData;

        [SerializeField] private Door doorTrm;

        private void Awake()
        {
            if(m_dialogueData == null) m_dialogueData = new DialogueDataSO();

            TalkingData talkingData = new TalkingData();
            talkingData.talker = m_talker;
            talkingData.dialogueMessage = m_scripts;
            m_dialogueData.dialougueDatas.Add(talkingData);
        }
        public void Interact()
        {
            if (DialogueManager.Instance.IsDialogueOpen) return;

            DialogueManager.Instance.StartDialogue(m_dialogueData);

            if(doorTrm is not null)
            {
                doorTrm.UpDoor();
            }
        }
    }
}
