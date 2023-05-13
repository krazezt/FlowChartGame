using UnityEngine;

[ExecuteInEditMode]
public class SBParallelogram : ShapeBehavior {

    [Header("Shape Attributes")]
    public float width = 1;

    public float height = 0.5f;

    public float deviation = 0.3f;

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }

    public override void RenderShapeOutline() {
        lineRenderer.positionCount = 5;

        Vector3[] pointsPos = new Vector3[5];

        pointsPos[0] = new(-width + deviation, height, 0);
        pointsPos[1] = new(width + deviation, height, 0);
        pointsPos[2] = new(width - deviation, -height, 0);
        pointsPos[3] = new(-width - deviation, -height, 0);
        pointsPos[4] = new(-width + deviation, height, 0);

        lineRenderer.SetPositions(pointsPos);
    }

    public override Vector3 CalculateIntersectPosition() {
        if (testLine != null) {
            testLine.positionCount = 2;
            testLine.SetPosition(0, connectPoint);
            testLine.SetPosition(1, transform.position);
        }

        float phi = Mathf.Atan((connectPoint.y - transform.position.y) / (connectPoint.x - transform.position.x));

        if (Mathf.Abs(height / (width + deviation)) >= Mathf.Abs((connectPoint.y - transform.position.y) / (connectPoint.x - transform.position.x))) {    // Intersect position is on the circular line
            float a = width * Mathf.Cos(phi);
            float b = width * Mathf.Sin(phi);

            float phi1 = Mathf.Atan((((connectPoint.y - transform.position.y) > 0) ? height : -height) / (((connectPoint.x - transform.position.x) > 0) ? deviation : -deviation));
            float phi2 = Mathf.PI - (Mathf.PI/2 - phi) - phi1;

            Debug.Log("eulePhi = " + phi + ", eulePhi1 = " + phi1 + ", eulePhi2 = " + phi2);

            float c = b * Mathf.Tan(phi2);
            float distance = a + c;

            if (connectPoint.x - transform.position.x > 0)
                intersectPoint = transform.TransformPoint(Quaternion.Euler(0, 0, phi * 180 / Mathf.PI) * new Vector3(distance, 0, 0));
            else
                intersectPoint = transform.TransformPoint(Quaternion.Euler(0, 0, phi * 180 / Mathf.PI) * new Vector3(-distance, 0, 0));

            if (intersectObj != null)
                intersectObj.transform.position = intersectPoint;
        } else {
            if (intersectObj != null)
                intersectObj.transform.position = transform.position;
        }

        return intersectPoint;
    }
}