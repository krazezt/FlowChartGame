using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    [Serializable]
    public enum Popup {
        Win,
        Lose,
        Settings,
    }

    public CanvasGroup backdrop;
    public PopupBase[] popups;
    public Button customCasePairButton;

    public List<Sprite> testCaseStateSprites;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        };

        HidePopup();
    }

    public void ShowPopup(Popup popup) {
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

    public void HidePopup() {
        backdrop.DOFade(0f, GameConfig.POPUP_DURATION).OnComplete(() => {
            backdrop.blocksRaycasts = false;
        });
        foreach (PopupBase popup in popups)
            popup.Hide();
    }

    public void OnStartSimulate() {
        customCasePairButton.interactable = false;
    }

    public void OnStopSimulate() {
        customCasePairButton.interactable = true;
    }

    public void LoadNextLevel() {
        GameManager.instance.LoadNextLevel();
    }
}