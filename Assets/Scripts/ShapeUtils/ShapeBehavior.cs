using UnityEngine;

public class ShapeBehavior : MonoBehaviour {
    public Vector3 connectPoint;
    public GameObject connectObj;
    public GameObject intersectObj;
    protected LineRenderer lineRenderer;

    [Header("Tests")]
    public LineRenderer testLine;

    [HideInInspector]
    public Vector3 intersectPoint;

    protected virtual void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
    }

    protected virtual void Update() {
        if (connectObj != null) {
            connectPoint = connectObj.transform.position;
        }

        RenderShapeOutline();
        CalculateIntersectPosition();
    }

    public virtual void RenderShapeOutline() {
    }

    public virtual Vector3 CalculateIntersectPosition() {
        return Vector3.zero;
    }
}