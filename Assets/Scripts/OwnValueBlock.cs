using UnityEngine;

public class OwnValueBlock : MonoBehaviour {

    [SerializeField]
    protected int OutputValue;

    public int GetOutputValue() {
        return OutputValue;
    }
}