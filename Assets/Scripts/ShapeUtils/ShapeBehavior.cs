using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ShapeBehavior : MonoBehaviour {
    public GameObject connectObj;
    public LineRenderer lineRenderer;

    [Header("Tests")]
    public LineRenderer testLine;

    public GameObject intersectObj;

    [HideInInspector]
    public Vector3 intersectPoint;

    protected virtual void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
    }

    protected virtual void Update() {
        RenderShapeOutline();

        if (connectObj != null && testLine != null && intersectObj != null)
            CalculateIntersectPosition(connectObj.transform.position);
    }

    public virtual void RenderShapeOutline() {
    }

    public virtual Vector3 CalculateIntersectPosition(Vector3 connectPoint) {
        return Vector3.zero;
    }

    public virtual void UpdateIntersectObjPos() {
        if (intersectObj != null)
            intersectObj.transform.position = intersectPoint;
    }
}