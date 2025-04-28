using UnityEngine;

namespace Swift_Blade
{
    public class StatInfoController : MonoBehaviour
    {
        private StatInfoUI[] _statInfos;
        private PlayerStatCompo _playerStatCompo;

        private void Start()
        {
            _statInfos = GetComponentsInChildren<StatInfoUI>();
            _playerStatCompo = Player.Instance.GetEntityComponent<PlayerStatCompo>();

            _playerStatCompo.OnStatChanged += UpdateStatInfos;
            _playerStatCompo.ColorValueChangedAction += UpdateStatInfos;

            UpdateStatInfos();
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
