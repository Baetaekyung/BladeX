using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class StatInfoController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup statInfoPanel;

        private StatInfoUI[] _statInfos;
        private PlayerStatCompo _playerStatCompo;

        private bool _isStatShowed = false;

        private void Start()
        {
            _statInfos = GetComponentsInChildren<StatInfoUI>();
            _playerStatCompo = Player.Instance.GetEntityComponent<PlayerStatCompo>();

            _playerStatCompo.OnStatChanged += UpdateStatInfos;
            _playerStatCompo.ColorValueChangedAction += UpdateStatInfos;

            UpdateStatInfos();

            statInfoPanel.DOFade(0, 0f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                _isStatShowed = !_isStatShowed;
                float fade = _isStatShowed ? 1f : 0f;

                statInfoPanel.DOFade(fade, 0.3f).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            }
        }

        private void OnDestroy()
        {
            _playerStatCompo.OnStatChanged -= UpdateStatInfos;
        }

        private void UpdateStatInfos()
        {
            for (int i = 0; i < _statInfos.Length; i++)
                _statInfos[i].gameObject.SetActive(false);

            StatSO[] stats = StatComponent.GetAllStats();
            for (int i = 0; i < stats.Length; i++)
            {
                _statInfos[i].gameObject.SetActive(true);
                _statInfos[i].SetUI(stats[i]);
            }
        }
    }
}
