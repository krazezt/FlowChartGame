using UnityEngine;

[ExecuteInEditMode]
public class ConnectLineController : MonoBehaviour {
    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject endPoint;

    private const int POINT_COUNT = 2;

    private LineRenderer lineRenderer;

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
        lineRenderer.SetPosition(0, startPoint.transform.position);
        lineRenderer.SetPosition(1, endPoint.transform.position);
    }
}