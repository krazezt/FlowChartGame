using UnityEngine;

public class VirtualLine : MonoBehaviour {
    private Vector3 startPoint;
    private Vector3 endPoint;

    private const int POINT_COUNT = 2;

    private LineRenderer lineRenderer;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void Update() {
        if (!lineRenderer.enabled)
            return;

        if (startPoint == null || endPoint == null)
            return;

        UpdateEndPointFollowMouse();

        lineRenderer.positionCount = POINT_COUNT;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }

    private void UpdateEndPointFollowMouse() {
        Vector3 mouseScreenPos = Input.mousePosition;

        mouseScreenPos.z = Camera.main.WorldToScreenPoint(startPoint).z;
        endPoint = Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    public void SetStartPoint(Vector3 startPoint) {
        this.startPoint = startPoint;
    }

    public void Show() {
        lineRenderer.enabled = true;
    }

    public void Hide() {
        lineRenderer.enabled = false;
    }
}