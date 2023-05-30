using DG.Tweening;
using UnityEngine;

public class PopupBase : MonoBehaviour {

    public virtual void Show() {
        gameObject.SetActive(true);
        transform.localScale = GameConfig.POPUP_START_SCALE * Vector3.one;
        transform.DOScale(Vector3.one, GameConfig.POPUP_DURATION).SetEase(Ease.OutBack);
    }

    public virtual void Hide() {
        transform.DOScale(Vector3.zero, GameConfig.POPUP_DURATION).SetEase(Ease.InBack).OnComplete(() => { gameObject.SetActive(false); });
    }
}