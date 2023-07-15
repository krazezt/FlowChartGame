using UnityEngine;

public class PageInPopup : MonoBehaviour {

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}