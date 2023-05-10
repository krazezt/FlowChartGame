using UnityEngine;

public class CoreBlock : MonoBehaviour {

    // States
    [HideInInspector]
    public bool onDragging;

    // Variables
    private Vector3 offsetVector;

    private Vector3 firstPosition;

    private Vector3 offsetDragAddition = new(0, 0, 1);

    private void Awake() {
        onDragging = false;
    }

    private void OnMouseDown() {
        firstPosition = transform.position;
        offsetVector = transform.position - MouseWorldPositionStart();
        onDragging = true;
    }

    private void OnMouseDrag() {
        Vector3 fixedPosition = MouseWorldPositionDrag() + offsetVector;
        fixedPosition.z = firstPosition.z;
        fixedPosition += offsetDragAddition;

        transform.position = fixedPosition;
    }

    private void OnMouseUp() {
        transform.position = transform.position - offsetDragAddition;

        onDragging = false;
    }

    private Vector3 MouseWorldPositionStart() {
        Vector3 mouseScreenPos = Input.mousePosition;

        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    private Vector3 MouseWorldPositionDrag() {
        Vector3 mouseScreenPos = Input.mousePosition;

        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position - offsetDragAddition).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}