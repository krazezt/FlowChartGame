using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TestCase : MonoBehaviour {
    [Serializable]
    public class CasePair {
        public int value;
        public VariableBlock variableBlock;
    }

    public List<CasePair> casePairs;

    public enum State {
        None,
        Checking,
        Passed,
        Failed
    }

    public State state;
    private Image image;

    private void Start() {
        image = GetComponent<Image>();
        ResetState();
    }

    public virtual void ResetState() {
        Debug.Log(UIManager.instance.testCaseStateSprites.Count);
        image.sprite = UIManager.instance.testCaseStateSprites[(int)State.None];
        state = State.None;
    }

    public virtual void MarkAsChecking() {
        image.sprite = UIManager.instance.testCaseStateSprites[(int)State.Checking];
        state = State.Checking;
    }

    public virtual void MarkAsPassed() {
        image.sprite = UIManager.instance.testCaseStateSprites[(int)State.Passed];
        state = State.Passed;
    }

    public virtual void MarkAsFailed() {
        image.sprite = UIManager.instance.testCaseStateSprites[(int)State.Failed];
        state = State.Failed;
    }

    public virtual void SetupTestCase() {
        foreach (CasePair casePair in casePairs) {
            casePair.variableBlock.AssignOutputValue(casePair.value);
        }
    }
}