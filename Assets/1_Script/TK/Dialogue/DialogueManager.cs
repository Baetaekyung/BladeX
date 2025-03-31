using System;
using System.Collections;
using System.Text;
using UnityEngine;

namespace Swift_Blade
{
    public class DialogueManager : MonoSingleton<DialogueManager>
    { 
        [SerializeField] private DialogueUI dialogueUI;
        
        #region For Optimization

        private Coroutine      _dialogueRoutine;
        private WaitForSeconds _dialougeWaitTime;
        private StringBuilder  _sb = new StringBuilder();

        #endregion

        private bool _isDialogueOpen      = false;
        private bool _isForcedCancel      = false; //Esc로 다이얼로그를 중단하였는가?
        private bool _isForcedMessageSkip = false; //Enter를 눌러 내용을 한번에 출력하였는가?
        
        private string _currentDialogueMessage;
        
        private event Action _onAcceptEvent = null;

        public bool IsDialogueOpen => _isDialogueOpen; //다이얼 로그 열려있으면 Esc눌러도 설정창 안나오게
        
        private void Update()
        {
            if (IsDialogueOpen == false)
                return;

            SkipDialogueMessage();
        }

        public DialogueManager StartDialogue(DialogueDataSO dialogueData)
        {
            ResetDialogue();
            dialogueUI.ShowDialogue(EventHandler);

            return this;

            void EventHandler() => StartNewDialogue(dialogueData);
        }

        private void ResetDialogue()
        {
            _isForcedCancel = false;
            _isForcedMessageSkip = false;
        }

        private void StartNewDialogue(DialogueDataSO dialogueData)
        {
            dialogueUI.GetCancelButton.onClick.AddListener(CancelDialogue);
            dialogueUI.GetAcceptButton.onClick.RemoveAllListeners();
            dialogueUI.GetAcceptButton.gameObject.SetActive(false);
            dialogueUI.ClearMessageBox();
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

            dialogueUI.UnShowDialogue();
        }

        private IEnumerator DialogueRoutine(DialogueDataSO dialogueData)
        {
            _isDialogueOpen = true;
            _dialougeWaitTime = new WaitForSeconds(dialogueData.dialogueWaitTime);

            var messageLength   = dialogueData.dialougueDatas.Count;
            var dialogueProcess = 0;

            while (!_isForcedCancel && dialogueProcess < messageLength)
            {
                _isForcedMessageSkip = false;

                dialogueUI.ClearMessageBox(); //기존 메세지 지워주기
                _sb.Clear(); //기존 스트링 빌더 내용 지우기

                _currentDialogueMessage = dialogueData.dialougueDatas[dialogueProcess].dialogueMessage;
                dialogueUI.SetTalker(dialogueData.dialougueDatas[dialogueProcess].talker);

                bool isLastMessage    = dialogueProcess == messageLength - 1;
                var maxMessageProcess = dialogueData.dialougueDatas[dialogueProcess].dialogueMessage.Length;
                var messageProcess    = 0;

                while (!_isForcedMessageSkip
                    && messageProcess < maxMessageProcess) //문자 하나씩 출력 (dialogueSpeed based)
                {
                    _sb.Append(dialogueData.dialougueDatas[dialogueProcess]
                        .dialogueMessage[messageProcess]);

                    messageProcess++; //문자열 출력 진행상황 업데이트.
                    dialogueUI.SetMessage(_sb.ToString());

                    yield return _dialougeWaitTime;
                }

                if (isLastMessage)
                {
                    Accept(dialogueData);

                    //Press enter or click button is trigger of accept
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
                    dialogueUI.GetAcceptButton.onClick?.Invoke();
                }
                
                yield return new WaitUntil(() => _isForcedCancel || Input.GetKeyDown(KeyCode.Return));
                
                ++dialogueProcess;
            }
        }

        private void Accept(DialogueDataSO dialogueData)
        {   
            dialogueUI.GetAcceptButton.gameObject.SetActive(true);

            dialogueUI.GetAcceptButton.onClick.AddListener(InvokeAcceptEvent);
            dialogueUI.GetAcceptButton.onClick.AddListener(InvokeAllDialogueEvents);
            dialogueUI.GetAcceptButton.onClick.AddListener(CancelDialogue);

            void InvokeAllDialogueEvents()
            {
                foreach (DialogueEventSO dialogueEvent in dialogueData.dialogueEvent)
                    dialogueEvent?.InvokeEvent();
            }
        }

        private void InvokeAcceptEvent()
        {
            _onAcceptEvent?.Invoke();
            _onAcceptEvent = null;
        }

        public void CancelDialogue()
        {
            if (_isDialogueOpen == false)
                return;
            
            dialogueUI.GetCancelButton.onClick.RemoveAllListeners();
            dialogueUI.GetAcceptButton.onClick.RemoveAllListeners();

            dialogueUI.ClearMessageBox();
            _sb.Clear();
                    
            StopDialogue();
            _isForcedCancel = true; //강제 종료
        }

        private void SkipDialogueMessage()
        {
            if (_isDialogueOpen == false)
                return;
            
            if(Input.GetKeyDown(KeyCode.Return))
            {
                _isForcedMessageSkip = true; //강제 메세지 스킵

                dialogueUI.SetMessage(_currentDialogueMessage); //강제로 완성된 문자열 대입
            }
        }

        public void Subscribe(Action onAccept)
        {
            //_onAcceptEvent will be cleared after invoke.
            _onAcceptEvent = onAccept;
        }
    }
}
