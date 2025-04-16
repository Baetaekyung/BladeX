using UnityEngine;

namespace Swift_Blade
{
    public class ShakeCardObj : MonoBehaviour, IInteractable
    {
        [SerializeField] private UI.PopupType popupType = UI.PopupType.ColorMix;
        //[SerializeField] private string talker;
        //[SerializeField] private string subscripts;

        [SerializeField] private DialogueDataSO DialogueDataSO;

        private void Awake()
        {
            //if (m_dialogueData == null) m_dialogueData = new DialogueDataSO();
            //
            //TalkingData talkingData = new TalkingData();
            //talkingData.talker = talker;
            //talkingData.dialogueMessage = subscripts;
            //m_dialogueData.dialougueDatas.Add(talkingData);
        }

        public void Interact()
        {
            //if (DialogueManager.Instance.IsDialogueOpen) return;

            DialogueManager.Instance.StartDialogue(DialogueDataSO).Subscribe(HandleDialogueEndEvent);

            void HandleDialogueEndEvent()
            {
                PopupManager.Instance.PopUp(popupType);
            }
        }
    }
}
