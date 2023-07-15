using System.Collections.Generic;
using UnityEngine;

public class PagingPopup : PopupBase {
    [SerializeField] private List<PageInPopup> pages;

    public override void Show() {
        base.Show();

        pages[0].Show();
        for (int i = 1; i < pages.Count; i++) {
            pages[i].Hide();
        }
    }

    public void ShowPage(int page) {
        for (int i = 1; i < pages.Count; i++) {
            if (i == page)
                pages[i].Show();
            else
                pages[i].Hide();
        }
    }
}