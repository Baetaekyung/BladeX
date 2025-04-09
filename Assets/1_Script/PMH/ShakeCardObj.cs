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
                
            //��Ȥ�� �����̾�
            //������ �� �ƴ�
            //������ �� ��
            //ģ���� ���� ��
            //�λ�� ���
            //�������� ����� �����Ѱ�
        }
    }
}
