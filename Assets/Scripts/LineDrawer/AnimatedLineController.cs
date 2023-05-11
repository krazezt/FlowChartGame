using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimatedLineController : MonoBehaviour {
    [SerializeField] private float animationDuration = 2f ;
    [SerializeField] private List<GameObject> linePoints;

    private LineRenderer lineRenderer;
    private int pointsCount;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void Update() {
        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.Select(point => point.transform.position).ToArray());

        if (Input.GetKeyDown(KeyCode.P))
            StartAnimateLine();
    }

    public void ClearPoints() {
        linePoints.Clear();
    }

    public void AddPoint(GameObject newPoint) {
        linePoints.Add(newPoint);
    }

    public void StartAnimateLine() {
        lineRenderer.enabled = true;
        StartCoroutine(AnimateLine());
    }

    private IEnumerator AnimateLine() {
        pointsCount = linePoints.Count;
        float segmentDuration = animationDuration / pointsCount;

        for (int i = 0; i < pointsCount - 1; i++) {
            float startTime = Time.time;
            Vector3 startPosition = linePoints[i].transform.position;

            Vector3 endPosition = linePoints[i + 1].transform.position;

            Vector3 pos = startPosition;
            while (pos != endPosition) {
                float t = (Time.time - startTime) / segmentDuration;
                pos = Vector3.Lerp(startPosition, endPosition, t);

                // animate all other points except point at index i
                for (int j = i + 1; j < pointsCount; j++)
                    lineRenderer.SetPosition(j, pos);

                yield return null;
            }
        }

        yield return new WaitForSeconds(3);
        lineRenderer.enabled = false;
    }
}