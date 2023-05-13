using UnityEngine;

[ExecuteInEditMode]
public class SBRhombus : ShapeBehavior {

    [Header("Shape Attributes")]
    public float width = 1;

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
}