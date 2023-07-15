using UnityEngine;

[ExecuteInEditMode]
public class LoadingMark : MonoBehaviour {
    public float rotateSpeed = 180f;

    // Update is called once per frame
    private void Update() {
        transform.Rotate(-Time.deltaTime * rotateSpeed * Vector3.forward);
    }
}