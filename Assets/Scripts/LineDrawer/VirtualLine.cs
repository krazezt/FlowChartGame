using UnityEngine;

public class VirtualLine : ConnectLineController {
    private const int POINT_COUNT = 5;

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
        Vector3 startPos = startPoint.GetComponent<ShapeBehavior>().CalculateIntersectPosition(endPoint.transform.position);
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPoint.transform.position);
        if (labelText.activeSelf)
            labelText.transform.position = Vector3.Lerp(startPos, endPoint.transform.position, 0.5f);

        DrawArrow(endPoint.transform.position);
    }

    private void UpdateEndPointFollowMouse() {
        Vector3 mouseScreenPos = Input.mousePosition;

        mouseScreenPos.z = Camera.main.WorldToScreenPoint(startPoint.transform.position).z;
        endPoint.transform.position = Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    public void SetStartPoint(GameObject startPoint) {
        this.startPoint = startPoint;
    }

    public void Show(bool showLabel) {
        lineRenderer.enabled = true;

        //Debug.Log("startPoint: " + startPoint.name + " => " + showLabel);
        labelText.SetActive(showLabel);
    }

    public void Hide() {
        lineRenderer.enabled = false;
        labelText.SetActive(false);
    }
}