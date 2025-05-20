using Swift_Blade.Level;
using UnityEngine;

namespace Swift_Blade
{
    public class Memos : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform lightTrm;

        [SerializeField] private DialogueDataSO dialogueSO;

        private DialogueDataSO m_dialogueData;

        [SerializeField] private Door doorTrm;
        [SerializeField] private ParticleSystem[] fireworks;

        [SerializeField] private bool isIntroduceRooms = false;

        private void Awake()
        {
            if(m_dialogueData == null) m_dialogueData = Instantiate(dialogueSO);
            if(lightTrm != null) lightTrm.gameObject.SetActive(false);
            //
            //TalkingData talkingData = new TalkingData();
            //talkingData.talker = m_talker;
            //talkingData.dialogueMessage = m_scripts;
            //m_dialogueData.dialougueDatas.Clear();
            //m_dialogueData.dialougueDatas.Add(talkingData);
        }
        public void Interact()
        {
            if (DialogueManager.Instance.IsDialogueOpen) return;

            DialogueManager.Instance.StartDialogue(dialogueSO);
            if (lightTrm != null) lightTrm.gameObject.SetActive(true);

            /*
            void SecondDialogue()
            {
                if (m_secondScripts == "" && m_secondScripts is null) return;
                Debug.Log("µÎ¹ø¤Š");

                TalkingData talkingData = new TalkingData();
                talkingData.talker = m_talker;
                talkingData.dialogueMessage = m_secondScripts;
                m_dialogueData.dialougueDatas.Clear();
                m_dialogueData.dialougueDatas.Add(talkingData);

                DialogueManager.Instance.StartDialogue(m_dialogueData);
            }*/
            if (doorTrm != null)
            {
                doorTrm.UpDoor();

                foreach(var firework in fireworks)
                {
                    firework.Play();
                }
            }
        }
    }
}
