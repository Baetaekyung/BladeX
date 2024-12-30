using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Swift_Blade
{
    public class DialogManager : MonoSingleton<DialogManager>
    {
        [SerializeField] 
        private DialogUI _dialogUI;
        
        #region For Optimization

        private Coroutine _dialogRoutine;
        private WaitForSeconds _waitForSeconds;
        private StringBuilder _sb = new StringBuilder();

        #endregion
        
        [SerializeField] private DialogDataSO _testDialog;

        private bool _isDialogOpen = false;
        public bool IsDialogOpen => _isDialogOpen; //다이얼 로그 열려있으면 Esc눌러도 설정창 안나오게
        
        private bool _isForcedCancel = false; //Esc로 다이얼로그를 중단하였는가?
        private bool _isForcedMessageSkip = false; //Enter를 눌러 내용을 한번에 출력하였는가?

        private string _currentDialogMessage;

        private Action _onCompleteEvent = null;
        
        private void Update()
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                DoDialog(_testDialog).OnComplete(() => Debug.Log("다이얼로그 콜백 테스트"));
            }
            
            CancelByEscapeKey();
            SkipByEnterKey();
        }

        public DialogManager DoDialog(DialogDataSO dialogData)
        {
            _isForcedCancel = false;
            _isForcedMessageSkip = false;
            
            void StartDialogRoutine()
            {
                if(_dialogRoutine != null)
                    StopCoroutine(_dialogRoutine);
                
                _dialogRoutine = StartCoroutine(DialogRoutine(dialogData));
            }

            _dialogUI.ShowDialog(() =>
            {
                _dialogUI.ClearMessageBox();
                _sb.Clear();
                
                StartDialogRoutine();
            });

            return this;
        }

        public void StopDialog()
        {
            _isDialogOpen = false;
            
            _isForcedCancel = false;
            _isForcedMessageSkip = false;

            _dialogUI.UnShowDialog();
        }

        private IEnumerator DialogRoutine(DialogDataSO dialogData)
        {
            _isDialogOpen = true;
            
            _dialogUI.SetTalker(dialogData.talker);

            _waitForSeconds = new WaitForSeconds(dialogData.dialogSpeed);

            var messageLength = dialogData.dialogMessage.Count;
            var dialogProcess = 0;

            while (!_isForcedCancel && dialogProcess < messageLength)
            {
                _dialogUI.ClearMessageBox(); //기존 메세지 지워주기
                _sb.Clear(); //기존 스트링 빌더 내용 지우기

                _currentDialogMessage = dialogData.dialogMessage[dialogProcess];
                
                _isForcedMessageSkip = false;
                
                var maxMessageProcess = dialogData.dialogMessage[dialogProcess].Length;
                var messageProcess = 0;
                
                while (!_isForcedMessageSkip && messageProcess < maxMessageProcess) //문자 하나씩 출력 (dialogSpeed 기반)
                {
                    _sb.Append(dialogData.dialogMessage[dialogProcess][messageProcess]);
                    
                    messageProcess++; //문자열 출력 진행상황 업데이트.
                
                    _dialogUI.SetMessage(_sb.ToString());

                    yield return _waitForSeconds;
                }
                
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) || _isForcedCancel); 
                
                dialogProcess++; //다이얼 로그 진행상황 업데이트.
            }

            InvokeCompleteEvent();

            //이벤트 없거나 다이얼로그 강제 종료하면 끝.
            if (_isForcedCancel || dialogData.dialogEvent == null) yield break; 
            
            //이벤트들 실행
            foreach (DialogEventSO @event in dialogData.dialogEvent)
            {
                @event?.DoEvent();
            }
        }

        private void InvokeCompleteEvent()
        {
            _onCompleteEvent?.Invoke();
            _onCompleteEvent = null;
        }

        private void CancelByEscapeKey()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
                
            _dialogUI.ClearMessageBox();
            _sb.Clear();
                    
            StopDialog();
                    
            _isForcedCancel = true; //강제 종료
        }

        private void SkipByEnterKey()
        {
            if(!Input.GetKeyDown(KeyCode.Return)) return;
            
            _isForcedMessageSkip = true; //강제 메세지 스킵
            _dialogUI.SetMessage(_currentDialogMessage); //강제로 완성된 문자열 대입
        }

        public void OnComplete(Action onComplete)
        {
            _onCompleteEvent = null;
            
            _onCompleteEvent = onComplete;
        }
    }
}
