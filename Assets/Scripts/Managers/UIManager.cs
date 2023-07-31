using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    [Serializable]
    public enum Popup {
        Win,
        Lose,
        Request,
        Tutorials,
        FlowChartInfo,
        Settings,
    }

    public CanvasGroup loadingMask;
    public CanvasGroup backdrop;
    public PopupBase[] popups;
    public Button customCasePairButton;
    public Button stopSimulationButton;
    public Button simulateButton;
    public Button pauseButton;

    public TMP_Text requestText;
    public List<Sprite> testCaseStateSprites;

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

    public void ShowPopupDelay(Popup popup, float delay) {
        StartCoroutine(StartCountingToShowPopup(popup, delay));
    }

    private IEnumerator StartCountingToShowPopup(Popup popup, float delay) {
        yield return new WaitForSeconds(delay);
        ShowPopup(popup);
    }

    public void ShowLosePopup() {
        ShowPopup(Popup.Lose);
    }

    public void ShowWinPopup() {
        ShowPopup(Popup.Win);
    }

    public void ShowRequestPopup() {
        ShowPopup(Popup.Request);
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

    public void Pause() {
        GameManager.instance.Pause();
        simulateButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    public void UnPause() {
        GameManager.instance.UnPause();
        simulateButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    public void OnStartSimulate() {
        customCasePairButton.gameObject.SetActive(false);
        stopSimulationButton.gameObject.SetActive(true);
        simulateButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    public void OnStopSimulate() {
        customCasePairButton.gameObject.SetActive(true);
        stopSimulationButton.gameObject.SetActive(false);
        simulateButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    public void LoadNextLevel() {
        GameManager.instance.LoadNextLevel();
    }

    public void ShowLoadingScreen() {
        DOTween.RewindAll();
        DOTween.KillAll();

        loadingMask.blocksRaycasts = true;
        loadingMask.DOFade(1f, 0.5f).OnKill(() => loadingMask.alpha = 1f);
    }

    public void HideLoadingScreen() {
        DOTween.RewindAll();
        DOTween.KillAll();

        loadingMask.blocksRaycasts = false;
        loadingMask.DOFade(0f, 0.5f).OnKill(() => loadingMask.alpha = 0f);
    }

    public void SetRequestText(string text) {
        requestText.text = text;
    }

    public void SetSFXOn(bool sfxOn) {
        AudioManager.instance.SetSFXVolume(sfxOn ? AudioConfig.DEFAULT_SFX_VOLUME : 0f);
    }

    public void SetBGMOn(bool bgmOn) {
        AudioManager.instance.SetBGMVolume(bgmOn ? AudioConfig.DEFAULT_BGM_VOLUME : 0f);
    }

    public void OnButtonClick() {
        AudioManager.instance.PlaySFX_ButtonClick();
    }

    public void QuitToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}