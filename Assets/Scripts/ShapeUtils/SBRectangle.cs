using UnityEngine;

[ExecuteInEditMode]
public class SBRectangle : ShapeBehavior {

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

        pointsPos[0] = new(-width, height, 0);
        pointsPos[1] = new(width, height, 0);
        pointsPos[2] = new(width, -height, 0);
        pointsPos[3] = new(-width, -height, 0);
        pointsPos[4] = new(-width, height, 0);

        lineRenderer.SetPositions(pointsPos);
    }

    public override Vector3 CalculateIntersectPosition(Vector3 connectPoint) {
        if (testLine != null) {
            testLine.positionCount = 2;
            testLine.SetPosition(0, connectPoint);
            testLine.SetPosition(1, intersectPoint);
        }

        if (Mathf.Abs(height / width) >= Mathf.Abs((connectPoint.y - transform.position.y) / (connectPoint.x - transform.position.x))) {    // Intersect position is on the side edges
            Vector3 localPos = new() {
                x = connectPoint.x - transform.position.x > 0 ? width : - width,
                y = Mathf.Abs(width / (connectPoint.x - transform.position.x)) * (connectPoint.y - transform.position.y),
                z = 0,
            };

            intersectPoint = transform.TransformPoint(localPos);
        } else {
            Vector3 localPos = new() {
                x = Mathf.Abs(height / (connectPoint.y - transform.position.y)) * (connectPoint.x - transform.position.x),
                y = connectPoint.y - transform.position.y > 0 ? height : - height,
                z = 0,
            };

            intersectPoint = transform.TransformPoint(localPos);
        }

        UpdateIntersectObjPos();

        return intersectPoint;
    }
}