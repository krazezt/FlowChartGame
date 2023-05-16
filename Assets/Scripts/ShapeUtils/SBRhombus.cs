using UnityEngine;

[ExecuteInEditMode]
public class SBRhombus : ShapeBehavior {

    [Header("Shape Attributes")]
    [Range(0.1f, 10f)]
    public float width = 1;

    [Range(0.1f, 10f)]
    public float height = 0.5f;

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }

    public override void RenderShapeOutline() {
        lineRenderer.positionCount = 5;

        Vector3[] pointsPos = new Vector3[5];

        pointsPos[0] = new(-width, 0, 0);
        pointsPos[1] = new(0, height, 0);
        pointsPos[2] = new(width, 0, 0);
        pointsPos[3] = new(0, -height, 0);
        pointsPos[4] = new(-width, 0, 0);

        lineRenderer.SetPositions(pointsPos);
    }

    public override Vector3 CalculateIntersectPosition(Vector3 connectPoint) {
        if (testLine != null) {
            testLine.positionCount = 2;
            testLine.SetPosition(0, connectPoint);
            testLine.SetPosition(1, intersectPoint);
        }

        float phi = Mathf.Atan((connectPoint.y - transform.position.y) / (connectPoint.x - transform.position.x));
        float a = Mathf.Abs(width * Mathf.Sin(phi));
        float b = width * Mathf.Cos(phi);
        float phi1 = (connectPoint.y - transform.position.y > 0) ? Mathf.Atan(height / width) : Mathf.Atan(-height / width);
        float phi2 = Mathf.PI / 2 - Mathf.Abs(phi) - Mathf.Abs(phi1);
        float c = a * Mathf.Tan(phi2);
        float distance = b - c;

        // Debug.Log(string.Format("phi = {0}, a = {1}, b = {2}, phi1 = {3}, phi2 = {4}, c = {5},
        // distance = {6}", phi * Mathf.Rad2Deg, a, b, phi1 * Mathf.Rad2Deg, phi2 * Mathf.Rad2Deg,
        // c, distance));

        intersectPoint = transform.TransformPoint(Quaternion.Euler(0, 0, phi * Mathf.Rad2Deg) * new Vector3((connectPoint.x - transform.position.x >= 0) ? distance : -distance, 0, 0));

        UpdateIntersectObjPos();

        return intersectPoint;
    }
}