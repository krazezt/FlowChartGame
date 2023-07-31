using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerMainMenu : MonoBehaviour {
    public static UIManagerMainMenu instance;

    [Serializable]
    public enum Popup {
        Tutorials,
        FlowChartInfo,
        Settings,
    }

    public CanvasGroup backdrop;
    public PopupBase[] popups;

    private void Awake() {
        instance = this;

        HidePopup();
    }

    public void ShowPopup(Popup popup) {
        DOTween.RewindAll();
        DOTween.KillAll();

        backdrop.blocksRaycasts = true;
        backdrop.DOFade(GameConfig.BACKDROP_FADE_ALPHA, GameConfig.POPUP_DURATION);

        for (int i = 0; i < popups.Length; i++) {
            if (i == (int)popup)
                popups[i].Show();
            else
                popups[i].Hide();
        }
    }

    public void ShowTutorialsPopup() {
        ShowPopup(Popup.Tutorials);
    }

    public void ShowFlowChartInfoPopup() {
        ShowPopup(Popup.FlowChartInfo);
    }

    public void ShowSettingsPopup() {
        ShowPopup(Popup.Settings);
    }

    public void HidePopup() {
        DOTween.RewindAll();
        DOTween.KillAll();

        backdrop.DOFade(0f, GameConfig.POPUP_DURATION).OnComplete(() => {
            backdrop.blocksRaycasts = false;
        });
        foreach (PopupBase popup in popups)
            popup.Hide();
    }

    public void OnButtonClick() {
        AudioManager.instance.PlaySFX_ButtonClick();
    }

    public void StartPlay() {
        SceneManager.LoadScene("Gameplay");
    }
}