using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using System.Runtime.CompilerServices;

namespace Swift_Blade
{
    public class ColorMixer : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<ColorType, ColorRecorder> colorRecordDictionary = new();
        [SerializeField] private Image resultImage;
        [SerializeField] private UIEffect_ColorChange effect;

        private List<ColorType> _inputColors = new List<ColorType>(2);
        private int _currentInputCount = 0;

        private Tween _tween;

        private void Start()
        {
            resultImage.GetComponent<RotateUI>().SetRotate(false);
        }

        public void MixColor()
        {
            if(_inputColors.Count == 0)
            {
                PopupManager.Instance.LogMessage("������ �������� �ʾҽ��ϴ�", 1f);

                return;
            }

            if(CheckIsValidToMix(_inputColors))
            {
                ColorType colorType = ColorUtils.GetColor(_inputColors);

                //Decrease ingredient colors ex) make yellow 1, red -1, green -1
                DecreaseIngredientColors(_inputColors);

                //Increase mixed color value 1
                Player.Instance.GetEntityComponent<PlayerStatCompo>().IncreaseColorValue(colorType, 1);

                _inputColors.Clear();
                _currentInputCount = 0;

                HandleTweenKill();

                #region Animation
                // Animation
                DOVirtual.Float(0, 0.3f, 1.5f, (f) => effect.SetAlpha(f))
                    .SetEase(Ease.InBack)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

                DOVirtual.Float(0, 0.6f, 1.5f, (f) => effect.SetEff(ColorUtils.GetCustomColor(colorType), f))
                    .OnComplete(() =>
                    {
                        effect.Blink(1.5f,
                        () => PopupManager.Instance.LogMessage($"{colorType} �� ȹ��"));

                        effect.SetEff(Color.white, 1f);
                        effect.SetAlpha(0f);
                    })
                    .SetEase(Ease.InBack)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

                #endregion

                resultImage.GetComponent<RotateUI>().SetRotate(false);
                resultImage.transform.localRotation = Quaternion.identity;
                resultImage.DOColor(Color.white, 1f);
            }
            else
            {
                PopupManager.Instance.LogMessage("������ �ִ� ���� �����մϴ�.");
            }
        }

        //ingredients value -1
        private void DecreaseIngredientColors(IEnumerable<ColorType> colorList)
        {
            foreach(var color in colorList)
                GetColorTypeRecorder(color).DecreaseColor();
        }

        //check it is valid to mix, increased color
        private bool CheckIsValidToMix(IEnumerable<ColorType> colorList)
        {
            foreach (var color in colorList)
            {
                //every colors are valid. if not return false
                if (GetColorTypeRecorder(color).CheckValidToDecrease() == false)
                {
                    Debug.Log("���� ���� ����");
                    return false;
                }
            }

            return true;
        }

        public void InputColor(ColorType inputColor)
        {
            // ���� ������ �� ���� �߰� �߾��ٸ�
            if (_inputColors.Contains(inputColor))
            {
                CancelColor(inputColor);

                return;
            }

            // ���� �̹� 2���� ���� ���� �߾��ٸ�
            if (_currentInputCount == 2)
            {
                PopupManager.Instance.LogMessage("�̹� 2���� ���� �����Ͽ����ϴ�.", 1f);

                return;
            }

            // �� ���� �߰��� �� �ִ��� üũ�Ŀ� �߰�
            if(GetColorTypeRecorder(inputColor).CheckValidToDecrease())
            {
                _inputColors.Add(inputColor);
                _currentInputCount++;

                if(_currentInputCount == 2)
                {
                    resultImage.GetComponent<RotateUI>().SetRotate(true);
                }

                ColorType colorType = ColorUtils.GetColor(_inputColors);
                Color color = ColorUtils.GetCustomColor(colorType);

                HandleTweenKill();
                _tween = resultImage.DOColor(color, 1.5f);

                PopupManager.Instance.LogMessage($"{inputColor} ���� �߰�", 1f);
            }
        }

        #region ��ư �̺�Ʈ

        public void InputRedColor()
        {
            InputColor(ColorType.RED);
        }
        public void InputGreenColor()
        {
            InputColor(ColorType.GREEN);
        }
        public void InputBlueColor()
        {
            InputColor(ColorType.BLUE);
        }

        #endregion

        private void CancelColor(ColorType inputColor)
        {
            _inputColors.Remove(inputColor);
            _currentInputCount--;

            Color color = Color.white;
            if (_inputColors.Count > 0)
            {
                ColorType colorType = ColorUtils.GetColor(_inputColors);
                color = ColorUtils.GetCustomColor(colorType);
            }

            resultImage.GetComponent<RotateUI>().SetRotate(false);

            HandleTweenKill();
            _tween = resultImage.DOColor(color, 1.5f);

            PopupManager.Instance.LogMessage($"{inputColor} ���� ����", 1f);
        }

        private void HandleTweenKill()
        {
            if (_tween != null)
                _tween.Kill();
        }

        public ColorRecorder GetColorTypeRecorder(ColorType colorType)
        {
            if (colorRecordDictionary.TryGetValue(colorType, out var colorRecorder))
                return colorRecorder;

            Debug.LogWarning("Color recorder is not exist in dictionary", transform);
            return null;
        }
    }
}
