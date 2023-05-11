using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    [SerializeField]
    private AnimatedLineController resultDrawLine;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }

    public void AppendResultLinePoint(GameObject obj) {
        resultDrawLine.AddPoint(obj);
    }

    public void ClearResultPoint() {
        resultDrawLine.ClearPoints();
    }
}