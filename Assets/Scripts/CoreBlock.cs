using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoreBlock : MonoBehaviour {

    public enum State {
        None,
        Holding,
        Drag,
        Connect
    }

    [SerializeField] protected ShapeBehavior shapeBehavior;

    public TMP_Text labelText;
    [HideInInspector] public bool onDragging;

    private Vector3 offsetVector;
    private Vector3 firstPosition;
    private Vector3 firstTouchPosition;
    private Vector3 offsetDragAddition = new(0, 0, 1);

    private float deltaLatency;

    public State state;
    protected static List<CoreBlock> m_listConnection;

    protected virtual void Awake() {
        onDragging = false;
        deltaLatency = 0;
        if (!TryGetComponent(out shapeBehavior))
            shapeBehavior = gameObject.AddComponent<ShapeBehavior>();

        state = State.None;
        m_listConnection = new List<CoreBlock>();
    }

    private void OnMouseDown() {
        firstTouchPosition = MouseWorldPosition();
        deltaLatency = 0;
    }

    private void OnMouseDrag() {
        switch (state) {
            case State.None:
                state = State.Holding;
                break;

            case State.Holding:
                if ((MouseWorldPosition() - firstTouchPosition).magnitude < GameConfig.MAX_HOLD_OFFSET) {
                    if (deltaLatency > GameConfig.DRAG_LATENCY)
                        StartDrag();
                    else
                        deltaLatency += Time.deltaTime;
                } else {
                    StartConnect();
                }

                break;

            case State.Drag:
                Drag();
                break;

            case State.Connect:
                Connect();
                break;
        }
    }

    private void OnMouseUp() {
        switch (state) {
            case State.Drag:
                EndDrag();
                break;

            case State.Connect:
                EndConnect();
                break;

            case State.Holding:
            case State.None:
                break;
        }
    }

    private void OnMouseEnter() {
        m_listConnection.Add(this);
    }

    private void OnMouseExit() {
        m_listConnection.Remove(this);
    }

    public void SetLabel(string label) {
        labelText.text = label;
    }

    private void StartDrag() {
        firstPosition = transform.position;
        offsetVector = transform.position - MouseWorldPosition();

        state = State.Drag;
        onDragging = true;
    }

    private void Drag() {
        Vector3 fixedPosition = MouseWorldPositionDrag() + offsetVector;
        fixedPosition.z = firstPosition.z;
        fixedPosition += offsetDragAddition;

        transform.position = fixedPosition;
    }

    private void EndDrag() {
        transform.position = transform.position - offsetDragAddition;

        state = State.None;
        onDragging = false;
    }

    protected virtual void StartConnect() {
        GameManager.instance.ShowPrimaryVirtualLine(shapeBehavior.CalculateIntersectPosition(MouseWorldPosition()));

        state = State.Connect;
    }

    private void Connect() {
        GameManager.instance.primaryVirtualLine.SetStartPoint(shapeBehavior.CalculateIntersectPosition(MouseWorldPosition()));
    }

    protected virtual bool EndConnect() {
        GameManager.instance.HideVirtualLine();
        if (m_listConnection.Count > 0)
            if (m_listConnection[^1].gameObject != gameObject) {
                // Debug.Log("Connected: " + gameObject.name + " - " + m_listConnection[^1].gameObject.name);
            } else {
                state = State.None;
                return false;
            }
        else {
            state = State.None;
            return false;
        }

        state = State.None;
        return true;
    }

    protected Vector3 MouseWorldPosition() {
        Vector3 mouseScreenPos = Input.mousePosition;

        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    protected Vector3 MouseWorldPositionDrag() {
        Vector3 mouseScreenPos = Input.mousePosition;

        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position - offsetDragAddition).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}