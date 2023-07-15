using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour {
    [SerializeField] private RectTransform uiHandleRectTransform ;
    [SerializeField] private Color backgroundActiveColor ;
    [SerializeField] private Color handleActiveColor ;

    private Image backgroundImage, handleImage;

    private Color backgroundDefaultColor, handleDefaultColor;

    private Toggle toggle;

    private Vector2 handlePosition;

    private void Awake() {
        toggle = GetComponent<Toggle>();

        handlePosition = uiHandleRectTransform.anchoredPosition;

        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        handleImage = uiHandleRectTransform.GetComponent<Image>();

        backgroundDefaultColor = backgroundImage.color;
        handleDefaultColor = handleImage.color;

        toggle.onValueChanged.AddListener(OnSwitch);

        if (toggle.isOn)
            OnSwitchNoAnim(true);
    }

    private void OnSwitch(bool on) {
        uiHandleRectTransform.DOAnchorPos(on ? handlePosition * -1 : handlePosition, 0.4f).SetEase(Ease.InOutBack);
        backgroundImage.DOColor(on ? backgroundActiveColor : backgroundDefaultColor, 0.6f);
        handleImage.DOColor(on ? handleActiveColor : handleDefaultColor, 0.4f);
    }

    private void OnSwitchNoAnim(bool on) {
        uiHandleRectTransform.anchoredPosition = on ? handlePosition * -1 : handlePosition;
        backgroundImage.color = on ? backgroundActiveColor : backgroundDefaultColor;
        handleImage.color = on ? handleActiveColor : handleDefaultColor;
    }

    private void OnDestroy() {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}