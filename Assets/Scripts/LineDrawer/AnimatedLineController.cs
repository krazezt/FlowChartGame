using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimatedLineController : MonoBehaviour {
    public List<GameObject> linePoints;
    public bool OnAnimating;
    public GameObject processObject;

    private float segmentDuration;
    private LineRenderer lineRenderer;
    private int pointsCount;
    private GameObject processObj;

    private void Awake() {
        segmentDuration = GameConfig.VISUALIZE_SEGMENT_DURATION;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        OnAnimating = false;
    }

    private void Update() {
        lineRenderer.SetPositions(linePoints.Select(point => point.transform.position).ToArray());

        if (Input.GetKeyDown(KeyCode.P))
            StartAnimateLine();
    }

    public void ClearPoints() {
        Hide();
        linePoints.Clear();
        lineRenderer.positionCount = linePoints.Count;
    }

    public void AddPoint(GameObject newPoint) {
        Debug.Log(linePoints.Count);
        linePoints.Add(newPoint);
        lineRenderer.positionCount = linePoints.Count;
    }

    public void Show() {
        lineRenderer.enabled = true;
    }

    public void Hide() {
        lineRenderer.enabled = false;
    }

    public void StartAnimateLine() {
        lineRenderer.enabled = true;
        StartCoroutine(AnimateLine());
    }

    private IEnumerator AnimateLine() {
        processObj = Instantiate(processObject);
        OnAnimating = true;
        pointsCount = linePoints.Count;
        //float segmentDuration = animationDurationOneEdge / pointsCount;

        for (int i = 0; i < pointsCount - 1; i++) {
            float startTime = Time.time;
            Vector3 startPosition = linePoints[i].transform.position;

            Vector3 endPosition = linePoints[i + 1].transform.position;

            Vector3 pos = startPosition;
            GameManager.instance.DequeueVariableLog();
            while (pos != endPosition) {
                float t = (Time.time - startTime) / segmentDuration;
                pos = Vector3.Lerp(startPosition, endPosition, t);

                // animate all other points except point at index i
                for (int j = i + 1; j < pointsCount; j++) {
                    lineRenderer.SetPosition(j, pos);
                }
                processObj.transform.position = pos;

                yield return null;
            }
        }

        yield return new WaitForSeconds(1);
        StopAnimating();
    }

    public void StopAnimating() {
        StopAllCoroutines();
        lineRenderer.enabled = false;
        Destroy(processObj);
        OnAnimating = false;
    }
}