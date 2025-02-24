using System;
using DG.Tweening;
using Swift_Blade.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI dialogMessageText;
        [SerializeField] private TextMeshProUGUI talkerText;
        public CanvasGroup canvasGroup;

        public void ShowDialog() //콜백 없음
        {
            canvasGroup.DOFade(1, 0.2f);
        }

        public void ShowDialog(Action callback) //콜백 있음
        {
            //어써트 넣기
            //Debug.Assert(callback != null)
            canvasGroup.DOFade(1, 0.2f).OnComplete(() => callback.Invoke());
        }

        public void UnShowDialog()
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
    }
}
