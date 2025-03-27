using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI dialogMessageText;
        [SerializeField] private TextMeshProUGUI talkerText;
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private Button acceptButton;
        [SerializeField] private Button cancelButton;

        public Button GetAcceptButton => acceptButton;
        public Button GetCancelButton => cancelButton;

        public void ShowDialog() //콜백 없음
        {
            canvasGroup.DOFade(1, 0.2f);
        }

        public void ShowDialogue(Action callback) //콜백 있음
        {
            //어써트 넣기
            //Debug.Assert(callback != null)
            Debug.Assert(callback != null, "Callback is null");
            canvasGroup.DOFade(1, 0.2f).OnComplete(() => callback.Invoke());
        }

        public void UnShowDialogue()
        {
            ClearMessageBox();
            ClearTalker();
            
            canvasGroup.DOFade(0, 0.2f);
        }

        public void SetMessage(string message)
        {
            dialogMessageText.text = message;
        }

        public void SetTalker(string talker)
        {
            talkerText.text = talker;
        }
        
        public void ClearMessageBox()
        {
            dialogMessageText.text = "";
        }

        private void ClearTalker()
        {
            talkerText.text = "";
        }
        
        private void OnDisable()
        {
            acceptButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
        }
    }
}
