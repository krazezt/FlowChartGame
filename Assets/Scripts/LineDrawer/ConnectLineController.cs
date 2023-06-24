using UnityEngine;

public class ConnectLineController : MonoBehaviour {
    public GameObject startPoint;
    public GameObject endPoint;
    public float arrowPhi = 15;
    public float arrowLength = 0.1f;
    public GameObject labelText;

    private const int POINT_COUNT = 5;

    protected LineRenderer lineRenderer;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void Update() {
        if (startPoint == null || endPoint == null)
            return;

        if (!lineRenderer.enabled)
            lineRenderer.enabled = true;

        lineRenderer.positionCount = POINT_COUNT;
        lineRenderer.SetPosition(0, startPoint.GetComponent<ShapeBehavior>().CalculateIntersectPosition(endPoint.transform.position));
        lineRenderer.SetPosition(1, endPoint.GetComponent<ShapeBehavior>().CalculateIntersectPosition(startPoint.transform.position));
        if (labelText.activeSelf)
            labelText.transform.position = Vector3.Lerp(startPoint.transform.position, endPoint.transform.position, 0.5f);

        DrawArrow(endPoint.GetComponent<ShapeBehavior>().CalculateIntersectPosition(startPoint.transform.position));
    }

    protected void DrawArrow(Vector3 pointer) {
        float phi = Mathf.Atan((pointer.y - startPoint.transform.position.y) / (pointer.x - startPoint.transform.position.x));
        float phi1 = phi + arrowPhi * Mathf.Deg2Rad;
        float phi2 = phi - arrowPhi * Mathf.Deg2Rad;

        Vector3 offset1 = Quaternion.Euler(0, 0, phi1 * Mathf.Rad2Deg) * new Vector3(-arrowLength, 0, 0);
        Vector3 offset2 = Quaternion.Euler(0, 0, phi2 * Mathf.Rad2Deg) * new Vector3(-arrowLength, 0, 0);

        if (pointer.x - startPoint.transform.position.x >= 0) {
            lineRenderer.SetPosition(2, pointer + offset1);
            lineRenderer.SetPosition(3, pointer + offset2);
            lineRenderer.SetPosition(4, pointer);
        } else {
            lineRenderer.SetPosition(2, pointer - offset1);
            lineRenderer.SetPosition(3, pointer - offset2);
            lineRenderer.SetPosition(4, pointer);
        }
    }
}