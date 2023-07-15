using DG.Tweening;
using UnityEngine;

public class PopupBase : MonoBehaviour {

    public virtual void Show() {
        Debug.Log("Show: " + gameObject);
        gameObject.SetActive(true);
        transform.localScale = GameConfig.POPUP_START_SCALE * Vector3.one;
        transform.DOScale(Vector3.one, GameConfig.POPUP_DURATION).SetEase(Ease.OutBack).OnKill(() => {
            gameObject.SetActive(true);
            transform.localScale = Vector3.one;
        });
    }

    public virtual void Hide() {
        transform.DOScale(Vector3.zero, GameConfig.POPUP_DURATION).SetEase(Ease.InBack).OnComplete(() => { gameObject.SetActive(false); }).OnKill(() => {
            gameObject.SetActive(false);
        });
    }
}