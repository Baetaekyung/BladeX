using UnityEngine;

namespace Swift_Blade
{
    public class ShakeCardObj : MonoBehaviour, IInteractable
    {
        [SerializeField] private UI.PopupType popupType = UI.PopupType.ColorMix;
        //[SerializeField] private string talker;
        //[SerializeField] private string subscripts;

        [SerializeField] private DialogueDataSO DialogueDataSO;

        private DialogueDataSO m_dialogueData;

        private bool isShowed = false;  
        private void Awake()
        {
            isShowed = false;
            //if (m_dialogueData == null) m_dialogueData = new DialogueDataSO();
            //
            //TalkingData talkingData = new TalkingData();
            //talkingData.talker = talker;
            //talkingData.dialogueMessage = subscripts;
            //m_dialogueData.dialougueDatas.Add(talkingData);
        }
        private void OnEnable()
        {
            isShowed = false;
        }

        public void Interact()
        {
            //if (DialogueManager.Instance.IsDialogueOpen) return;

            if(isShowed)
            {
                Debug.Log($"Interacted + {transform.name}");
                PopupManager.Instance.PopUp(popupType);
            }
            else
            {
                DialogueManager.Instance.StartDialogue(DialogueDataSO);
                isShowed = true;
            }


            //HealingStatue
                
            //잔혹한 세상이야
            //눈에는 눈 아니
            //내눈에 눈 물
            //친절하 마음 엔
            //인사는 쥐뿔
            //언제부터 였을까내 가변한건
        }
    }
}
