using UnityEngine;

[ExecuteInEditMode]
public class SBCapsule : ShapeBehavior {

    [Header("Shape Attributes")]
    public float width = 0.5f;

    public float height = 0.5f;

    [Range(4, 100)]
    public int pointsCount = 30;

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }

    public override void RenderShapeOutline() {
        if (pointsCount % 2 == 0)
            lineRenderer.positionCount = pointsCount + 5;
        else
            lineRenderer.positionCount = pointsCount + 4;

        Vector3[] pointsPos = new Vector3[lineRenderer.positionCount];
        Vector3 offsetRightCenter = new(width, 0, 0);
        Vector3 offsetCircular = new(0, height, 0);
        int index = 0;
        int pointForEachSide = pointsCount / 2;

        pointsPos[index++] = new(-width, height, 0);
        pointsPos[index++] = new(width, height, 0);

        for (int i = 0; i < pointForEachSide; i++) {
            float delta = (i+1)/(float)(pointForEachSide + 1);
            Vector3 pointPos = offsetRightCenter + Quaternion.Euler(0, 0, -180 * delta) * offsetCircular;

            pointsPos[index++] = new(pointPos.x, pointPos.y, 0);
        }

        pointsPos[index++] = new(width, -height, 0);
        pointsPos[index++] = new(-width, -height, 0);

        for (int i = 0; i < pointForEachSide; i++) {
            float delta = (i+1)/(float)(pointForEachSide + 1);
            Vector3 pointPos = - offsetRightCenter - Quaternion.Euler(0, 0, -180 * delta) * offsetCircular;

            pointsPos[index++] = new(pointPos.x, pointPos.y, 0);
        }

        pointsPos[index] = new(-width, height, 0);

        lineRenderer.SetPositions(pointsPos);
    }

    public override Vector3 CalculateIntersectPosition() {
        if (testLine != null) {
            testLine.positionCount = 2;
            testLine.SetPosition(0, connectPoint);
            testLine.SetPosition(1, intersectPoint);
        }

        float phi = Mathf.Atan((connectPoint.y - transform.position.y) / (connectPoint.x - transform.position.x));

        if (Mathf.Abs(height / width) >= Mathf.Abs((connectPoint.y - transform.position.y) / (connectPoint.x - transform.position.x))) {    // Intersect position is on the circular line
            float a = width * Mathf.Cos(phi);
            float b = width * Mathf.Sin(phi);
            float c = Mathf.Sqrt(height*height - b*b);
            float distance = a + c;

            if (connectPoint.x - transform.position.x > 0)
                intersectPoint = transform.TransformPoint(Quaternion.Euler(0, 0, phi * 180 / Mathf.PI) * new Vector3(distance, 0, 0));
            else
                intersectPoint = transform.TransformPoint(Quaternion.Euler(0, 0, phi * 180 / Mathf.PI) * new Vector3(-distance, 0, 0));

            if (intersectObj != null)
                intersectObj.transform.position = intersectPoint;
        } else {
            Vector3 localPos = new() {
                x = Mathf.Abs(height / (connectPoint.y - transform.position.y)) * (connectPoint.x - transform.position.x),
                y = connectPoint.y - transform.position.y > 0 ? height : - height,
                z = 0,
            };

            intersectPoint = transform.TransformPoint(localPos);

            if (intersectObj != null)
                intersectObj.transform.position = intersectPoint;
        }

        return intersectPoint;
    }
}