using UnityEngine;

[ExecuteInEditMode]
public class SBParallelogram : ShapeBehavior {

    [Header("Shape Attributes")]
    [Range(0.1f, 10f)]
    public float width = 1;

    [Range(0.1f, 10f)]
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

    public override Vector3 CalculateIntersectPosition(Vector3 connectPoint) {
        if (testLine != null) {
            testLine.positionCount = 2;
            testLine.SetPosition(0, connectPoint);
            testLine.SetPosition(1, intersectPoint);
        }

        float phi = Mathf.Atan((connectPoint.y - transform.position.y) / (connectPoint.x - transform.position.x));

        if (phi >= 0) {                 // Intersect point is on the quadrant 1 or 3
            if (Mathf.Abs(height / (width + deviation)) >= Mathf.Abs((connectPoint.y - transform.position.y) / (connectPoint.x - transform.position.x))) {      // Intersect point is on the side edges
                float a = width * Mathf.Cos(phi);
                float b = width * Mathf.Sin(phi);

                float phi1 = Mathf.Atan((((connectPoint.y - transform.position.y) > 0) ? height : -height) / (((connectPoint.x - transform.position.x) > 0) ? deviation : -deviation));
                float phi2 = Mathf.PI - (Mathf.PI/2 - phi) - phi1;

                float c = b * Mathf.Tan(phi2);
                float distance = a + c;

                if (connectPoint.x - transform.position.x > 0)
                    intersectPoint = transform.TransformPoint(Quaternion.Euler(0, 0, phi * 180 / Mathf.PI) * new Vector3(distance, 0, 0));
                else
                    intersectPoint = transform.TransformPoint(Quaternion.Euler(0, 0, phi * 180 / Mathf.PI) * new Vector3(-distance, 0, 0));

                UpdateIntersectObjPos();
            } else {                    // Intersect point is on the top or bottom edge
                Vector3 localPos = new() {
                    x = Mathf.Abs(height / (connectPoint.y - transform.position.y)) * (connectPoint.x - transform.position.x),
                    y = connectPoint.y - transform.position.y > 0 ? height : - height,
                    z = 0,
                };

                intersectPoint = transform.TransformPoint(localPos);

                UpdateIntersectObjPos();
            }
        } else {                        // Intersect point is on the quadrant 2 or 4
            if (Mathf.Abs(height / (width - deviation)) >= Mathf.Abs((connectPoint.y - transform.position.y) / (connectPoint.x - transform.position.x))) {      // Intersect point is on the side edges
                float a = width * Mathf.Cos(phi);
                float b = width * Mathf.Sin(phi);

                float phi1 = Mathf.Atan((((connectPoint.y - transform.position.y) > 0) ? height : -height) / (((connectPoint.x - transform.position.x) > 0) ? deviation : -deviation));
                float phi2 = Mathf.PI/2 + phi + phi1;

                // Debug.Log("Eule phi = " + Mathf.Rad2Deg * phi + ", Eule phi1 = " + Mathf.Rad2Deg
                // * phi1 + ", Eule phi2 = " + Mathf.Rad2Deg * phi2);

                float c = b * Mathf.Tan(phi2);
                float distance = a + c;

                if (connectPoint.x - transform.position.x > 0)
                    intersectPoint = transform.TransformPoint(Quaternion.Euler(0, 0, phi * 180 / Mathf.PI) * new Vector3(distance, 0, 0));
                else
                    intersectPoint = transform.TransformPoint(Quaternion.Euler(0, 0, phi * 180 / Mathf.PI) * new Vector3(-distance, 0, 0));

                UpdateIntersectObjPos();
            } else {                    // Intersect point is on the top or bottom edge
                Vector3 localPos = new() {
                    x = Mathf.Abs(height / (connectPoint.y - transform.position.y)) * (connectPoint.x - transform.position.x),
                    y = connectPoint.y - transform.position.y > 0 ? height : - height,
                    z = 0,
                };

                intersectPoint = transform.TransformPoint(localPos);

                UpdateIntersectObjPos();
            }
        }

        return intersectPoint;
    }
}