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

            foreach(var stat in StatComponent.GetAllStats())
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

        private void OnDisable()
        {
            foreach (var stat in StatComponent.GetAllStats())
            {
                stat.OnValueChanged -= UpdateStatInfos;
            }
        }

        private void UpdateStatInfos()
        {
            for (int i = 0; i < _statInfos.Length; i++)
                _statInfos[i].gameObject.SetActive(false);

            for (int i = 0; i < StatComponent.GetAllStats().Length; i++)
            {
                _statInfos[i].gameObject.SetActive(true);
                _statInfos[i].SetUI(StatComponent.GetAllStats()[i]);
            }
        }
    }
}
