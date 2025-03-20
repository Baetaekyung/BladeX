using System;
using System.Collections;
using System.Text;
using UnityEngine;

namespace Swift_Blade
{
    public class DialogueManager : MonoSingleton<DialogueManager>
    { 
        [SerializeField] private DialogueUI _dialogueUI;
        
        #region For Optimization

        private Coroutine _dialogueRoutine;
        private WaitForSeconds _waitForSeconds;
        private StringBuilder _sb = new StringBuilder();

        #endregion
        
        [SerializeField] private DialogueDataSO testDialogue;

        private bool _isDialogueOpen = false;
        private bool _isForcedCancel = false; //Esc로 다이얼로그를 중단하였는가?
        private bool _isForcedMessageSkip = false; //Enter를 눌러 내용을 한번에 출력하였는가?
        
        private string _currentDialogueMessage;
        
        private event Action _onAcceptEvent = null;

        public bool IsDialogueOpen => _isDialogueOpen; //다이얼 로그 열려있으면 Esc눌러도 설정창 안나오게
        
        private void Update()
        {
            SkipDialogueMessage();

            if (Input.GetKeyDown(KeyCode.L))
                DoDialogue(testDialogue);
        }

        public DialogueManager DoDialogue(DialogueDataSO dialogueData)
        {
            ResetDialogue();
            _dialogueUI.ShowDialog(() => { StartNewDialogue(dialogueData); });

            return this;
        }

        private void ResetDialogue()
        {
            _isForcedCancel = false;
            _isForcedMessageSkip = false;
        }

        private void StartNewDialogue(DialogueDataSO dialogueData)
        {
            _dialogueUI.GetCancelButton.onClick.AddListener(CancelDialogue);
            _dialogueUI.GetAcceptButton.onClick.RemoveAllListeners();
            _dialogueUI.GetAcceptButton.gameObject.SetActive(false);
            _dialogueUI.ClearMessageBox();
            _sb.Clear();
                
            if(_dialogueRoutine != null)
                StopCoroutine(_dialogueRoutine);
                
            _dialogueRoutine = StartCoroutine(DialogueRoutine(dialogueData));
        }

        public void StopDialogue()
        {
            ResetDialogue();

            _isDialogueOpen = false;
            _onAcceptEvent = null;
            _dialogueUI.UnShowDialog();
        }

        private IEnumerator DialogueRoutine(DialogueDataSO dialogueData)
        {
            _isDialogueOpen = true;
            _waitForSeconds = new WaitForSeconds(dialogueData.dialogueSpeed);
            
            _dialogueUI.SetTalker(dialogueData.talker);

            var messageLength = dialogueData.dialogueMessage.Count;
            var dialogProcess = 0;

            while (!_isForcedCancel && dialogProcess < messageLength)
            {
                _isForcedMessageSkip = false;
                
                _dialogueUI.ClearMessageBox(); //기존 메세지 지워주기
                _sb.Clear(); //기존 스트링 빌더 내용 지우기

                _currentDialogueMessage = dialogueData.dialogueMessage[dialogProcess];
                var maxMessageProcess = dialogueData.dialogueMessage[dialogProcess].Length;
                var messageProcess = 0;
                
                while (!_isForcedMessageSkip && messageProcess < maxMessageProcess) //문자 하나씩 출력 (dialogSpeed 기반)
                {
                    _sb.Append(dialogueData.dialogueMessage[dialogProcess][messageProcess]);
                    messageProcess++; //문자열 출력 진행상황 업데이트.
                    _dialogueUI.SetMessage(_sb.ToString());

                    yield return _waitForSeconds;
                }
                
                if (dialogProcess == messageLength - 1)
                {
                    ClickAcceptButton(dialogueData);

                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
                    _dialogueUI.GetAcceptButton.onClick?.Invoke();
                }
                
                yield return new WaitUntil(() 
                        => _isForcedCancel || Input.GetKeyDown(KeyCode.Return));
                
                ++dialogProcess; //다이얼 로그 진행상황 업데이트.
            }
        }

        private void ClickAcceptButton(DialogueDataSO dialogueData)
        {
            void InvokeAllDialogueEvents()
            {
                foreach (DialogueEventSO dialogueEvent in dialogueData.dialogueEvent)
                    dialogueEvent?.InvokeEvent();
            }
            
            _dialogueUI.GetAcceptButton.gameObject.SetActive(true);
            _dialogueUI.GetAcceptButton.onClick.AddListener(InvokeAcceptEvent);
            _dialogueUI.GetAcceptButton.onClick.AddListener(InvokeAllDialogueEvents);
            _dialogueUI.GetAcceptButton.onClick.AddListener(CancelDialogue);
        }

        private void InvokeAcceptEvent()
        {
            _onAcceptEvent?.Invoke();
            _onAcceptEvent = null;
            
            _dialogueUI.GetAcceptButton.onClick.RemoveAllListeners();
        }

        public void CancelDialogue()
        {
            if (_isDialogueOpen == false)
                return;
            
            _dialogueUI.GetCancelButton.onClick.RemoveAllListeners();
            _dialogueUI.GetAcceptButton.onClick.RemoveAllListeners();
            _dialogueUI.ClearMessageBox();
            _sb.Clear();
                    
            StopDialogue();
            _isForcedCancel = true; //강제 종료
        }

        private void SkipDialogueMessage()
        {
            if (_isDialogueOpen == false)
                return;
            
            if(!Input.GetKeyDown(KeyCode.Return))
                return;
            
            _isForcedMessageSkip = true; //강제 메세지 스킵
            _dialogueUI.SetMessage(_currentDialogueMessage); //강제로 완성된 문자열 대입
        }

        public void Subscribe(Action onAccept)
        {
            _onAcceptEvent = onAccept;
        }
        public void Desubscribe(Action onAccept)
        {
            _onAcceptEvent -= onAccept;
        }
    }
}
