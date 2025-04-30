using DG.Tweening;
using UnityEngine;

namespace Swift_Blade
{
    public class StatInfoController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup statInfoPanel;

        private StatInfoUI[] _statInfos;
        private PlayerStatCompo _playerStatCompo;
        private StatSO[] _currentStats;

        private bool _isStatShowed = false;

        private void Start()
        {
            _statInfos = GetComponentsInChildren<StatInfoUI>();
            _playerStatCompo = Player.Instance.GetEntityComponent<PlayerStatCompo>();

            _playerStatCompo.OnStatChanged += UpdateStatInfos;
            _playerStatCompo.ColorValueChangedAction += UpdateStatInfos;

            _currentStats = StatComponent.GetAllStats();
            foreach(var stat in _currentStats)
            {
                stat.OnValueChanged += UpdateStatInfos;
            }

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
            foreach (var stat in _currentStats)
            {
                stat.OnValueChanged -= UpdateStatInfos;
            }
        }

        private void UpdateStatInfos()
        {
            for (int i = 0; i < _statInfos.Length; i++)
                _statInfos[i].gameObject.SetActive(false);

            for (int i = 0; i < _currentStats.Length; i++)
            {
                _statInfos[i].gameObject.SetActive(true);
                _statInfos[i].SetUI(_currentStats[i]);
            }
        }
    }
}
