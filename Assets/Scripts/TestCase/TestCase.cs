using UnityEngine;
using UnityEngine.UI;

public class TestCase : MonoBehaviour {
    public Sprite stateNoneSprite;
    public Sprite stateCheckingSprite;
    public Sprite statePassedSprite;
    public Sprite stateFailedSprite;

    public enum State {
        None,
        Checking,
        Passed,
        Failed
    }

    public State state;
    public Image image;

    private void Start() {
        image = GetComponent<Image>();
        ResetState();
    }

    public virtual void ResetState() {
        image.sprite = stateNoneSprite;
        state = State.None;
    }

    public virtual void MarkAsChecking() {
        image.sprite = stateCheckingSprite;
        state = State.Checking;
    }

    public virtual void MarkAsPassed() {
        image.sprite = statePassedSprite;
        state = State.Passed;
    }

    public virtual void MarkAsFailed() {
        image.sprite = stateFailedSprite;
        state = State.Failed;
    }

    public virtual void SetupTestCase() {
    }
}