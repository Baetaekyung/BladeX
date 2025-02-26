using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Swift_Blade
{
    public class DialogueManager : MonoSingleton<DialogueManager>
    { 
        [SerializeField] private DialogueUI _dialogueUI;
        
        #region For Optimization

        private Coroutine _dialogueRoutine;
        private WaitForSeconds _waitForSeconds;
        private StringBuilder _sb = new StringBuilder(2);

        #endregion
        
        [SerializeField] private DialogueDataSO testDialogue;

        private bool _isDialogueOpen = false;
        private bool _isForcedCancel = false; //Esc로 다이얼로그를 중단하였는가?
        private bool _isForcedMessageSkip = false; //Enter를 눌러 내용을 한번에 출력하였는가?

        private string _currentDialogueMessage;
        
        private Action _onCompleteEvent = null;

        public bool IsDialogueOpen => _isDialogueOpen; //다이얼 로그 열려있으면 Esc눌러도 설정창 안나오게
        
        private void Update()
        {
            CancelDialogue();
            SkipDialogueMessage();
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

            _onCompleteEvent = null;
            _dialogueUI.UnShowDialog();
        }

        private IEnumerator DialogueRoutine(DialogueDataSO dialogueData)
        {
            _isDialogueOpen = true;
            
            _dialogueUI.SetTalker(dialogueData.talker);

            _waitForSeconds = new WaitForSeconds(dialogueData.dialogueSpeed);

            var messageLength = dialogueData.dialogueMessage.Count;
            var dialogProcess = 0;

            while (!_isForcedCancel && dialogProcess < messageLength)
            {
                _dialogueUI.ClearMessageBox(); //기존 메세지 지워주기
                _sb.Clear(); //기존 스트링 빌더 내용 지우기

                _currentDialogueMessage = dialogueData.dialogueMessage[dialogProcess];
                
                _isForcedMessageSkip = false;
                
                var maxMessageProcess = dialogueData.dialogueMessage[dialogProcess].Length;
                var messageProcess = 0;
                
                while (!_isForcedMessageSkip && messageProcess < maxMessageProcess) //문자 하나씩 출력 (dialogSpeed 기반)
                {
                    _sb.Append(dialogueData.dialogueMessage[dialogProcess][messageProcess]);
                    
                    messageProcess++; //문자열 출력 진행상황 업데이트.
                
                    _dialogueUI.SetMessage(_sb.ToString());

                    yield return _waitForSeconds;
                }
                
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) || _isForcedCancel);
                
                dialogProcess++; //다이얼 로그 진행상황 업데이트.
            }

            InvokeCompleteEvent();

            //이벤트 없거나 다이얼로그 강제 종료하면 끝.
            if (_isForcedCancel || dialogueData.dialogueEvent == null) yield break; 
            
            //이벤트들 실행
            foreach (DialogueEventSO dialogueEvent in dialogueData.dialogueEvent)
            {
                dialogueEvent?.InvokeEvent();
            }
        }

        private void InvokeCompleteEvent()
        {
            _onCompleteEvent?.Invoke();
            _onCompleteEvent = null;
        }

        private void CancelDialogue()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
                
            _dialogueUI.ClearMessageBox();
            _sb.Clear();
                    
            StopDialogue();
                    
            _isForcedCancel = true; //강제 종료
        }

        private void SkipDialogueMessage()
        {
            if(!Input.GetKeyDown(KeyCode.Return)) return;
            
            _isForcedMessageSkip = true; //강제 메세지 스킵
            _dialogueUI.SetMessage(_currentDialogueMessage); //강제로 완성된 문자열 대입
        }

        public void OnComplete(Action onComplete)
        {
            if (onComplete == null)
                return;
            
            _onCompleteEvent = onComplete;
        }
    }
}
